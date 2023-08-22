using Space.Application.Behaviours;

namespace Space.Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();  
        services.AddMediatR(opt => opt.RegisterServicesFromAssembly(Reflection.Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(Reflection.Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Reflection.Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        return services;
    }
}
