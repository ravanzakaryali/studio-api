using AspNetCoreRateLimit;
using Microsoft.OpenApi.Models;
using Space.Application.Helper;
using Space.WebAPI.Filters;
using Space.WebAPI.Middlewares;
using System.Reflection;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyNullableConverter());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
                      builder =>
                      {
                          builder.WithOrigins("https://heroic-semifreddo-70a6b6.netlify.app", "https://ui-space.netlify.app", "http://localhost:3000", "https://hammerhead-app-ka4wt.ondigitalocean.app", "https://studio.code.az")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod()
                                                .AllowCredentials();
                      });
});


builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

builder.Services.Configure<ClientRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.HttpStatusCode = 429;
    options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "1s",
                Limit = 3
            },
            new RateLimitRule
            {
                Endpoint = "*",
                Period = "1m",
                Limit = 80
            },
            new RateLimitRule
            {
                Endpoint = "POST:/api/supports",
                Period= "5s",
                Limit=1,
            },
             new RateLimitRule
            {
                Endpoint = "POST:/api/supports",
                Period= "1m",
                Limit=5,
            },
               new RateLimitRule
            {
                Endpoint = "POST:/api/supports",
                Period= "1d",
                Limit=30,
            },
        };
});

builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    //Todo: Enum swager drop down menu
    config.OperationFilter<AuthenticationRequirementOperationFilter>();
    //config.IncludeXmlComments(Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".xml"));
    config.UseInlineDefinitionsForEnums();
});

var app = builder.Build();

app.UseTokenAuthetication();
app.UseRateLimit();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseExceptionMiddelware();
app.UseCors();

app.Run();
