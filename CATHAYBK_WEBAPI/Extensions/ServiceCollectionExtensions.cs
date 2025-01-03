using BasicEIP_Core.NLog;
using System.Reflection;
using CATHAYBK_Service.Base;

namespace TMSAPP_WEBAPI.Extensions
{
    /// <summary>
    /// DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Repositories 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.Load("CATHAYBK_Service");

            // AddRepositories(services, assembly);
            // AddServices(services, assembly);

            return services;
        }
        /// <summary>
        /// App Logging
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppLogging(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
            return services;
        }
        /*
        private static void AddServices(IServiceCollection services, Assembly assembly)
        {
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract &&
                            t.BaseType != null &&
                            t.BaseType.IsGenericType &&
                            t.BaseType.GetGenericTypeDefinition() == typeof(ServiceBase<>))
                .ToList();

            foreach (var type in serviceTypes)
            {
                services.AddScoped(type);
            }
        }
        */
        /*
        private static void AddRepositories(IServiceCollection services, Assembly assembly)
        {
            var repositoryTypes = assembly.GetTypes().Where(t => !t.IsAbstract &&
                                                         t.BaseType != null &&
                                                         t.BaseType.IsGenericType &&
                                                         t.BaseType.GetGenericTypeDefinition() == typeof(RepositoryBase<,>));

            foreach (var repositoryType in repositoryTypes)
            {
                var loggerInterface = typeof(IAppLogger<>).MakeGenericType(repositoryType);
                var loggerImplementation = typeof(AppLogger<>).MakeGenericType(repositoryType);

                services.AddSingleton(loggerInterface, loggerImplementation);
                services.AddScoped(repositoryType);
            }
        }
        */
    }
}
