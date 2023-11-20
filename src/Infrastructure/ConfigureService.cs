using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Space.Infrastructure.Services.SendEmailService;

namespace Space.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {


        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<ISpaceDbContext>(provider => provider.GetRequiredService<SpaceDbContext>());

        services.AddDbContext<SpaceDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("SqlServer"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)), ServiceLifetime.Transient);

        services.AddIdentity<User, Role>(opt =>
        {
            opt.User.RequireUniqueEmail = false;
            opt.SignIn.RequireConfirmedPhoneNumber = false;
            opt.SignIn.RequireConfirmedEmail = true;
            opt.Password.RequiredUniqueChars = 0;
            opt.Password.RequiredLength = 8;
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
        })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<SpaceDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            option.RequireHttpsMetadata = false;
            option.SaveToken = true;

            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = configuration.GetSection("Jwt:Audience").Value,
                ValidIssuer = configuration.GetSection("Jwt:Issuer").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:SecurityKey").Value)),
                ClockSkew = TimeSpan.Zero,
            };
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

		services.AddScoped<SendEmailService>();
		services.AddSingleton<IHostedService, EmailServiceLauncher>();

		services.AddStorage<LocalStorage>();

        return services;
    }
    static void AddStorage<T>(this IServiceCollection services) where T : StorageHelper, IStorage
    {
        services.AddScoped<IStorage, T>();
    }
}
