using System.Reflection;

namespace Space.Infrastructure.Services;

public static class RepositoryService
{
    public static void AddRepositories(this IServiceCollection services)
    {
        var repositoryTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IRepository<>)));

        var nonBaseRepos = repositoryTypes.Where(t => t != typeof(Repository<>));

        foreach (var repositoryType in nonBaseRepos)
        {
            var interfaces = repositoryType.GetInterfaces()
                .FirstOrDefault(c => !c.IsGenericType)
                        ?? throw new InvalidOperationException($"Repository '{repositoryType.Name}' must implement only one interface that implements IRepositoryBase<T>.");

            services.AddScoped(interfaces, repositoryType);

        }
    }
}
