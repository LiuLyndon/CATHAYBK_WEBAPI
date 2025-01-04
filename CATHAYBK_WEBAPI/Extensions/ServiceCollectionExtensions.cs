using BasicEIP_Core.NLog;
using CATHAYBK_Service.Repositories;
using System.Reflection;
using TMSERP_Service.Base;

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

            AddServices(services, assembly);

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

        /// <summary>
        /// AddServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
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
    }
}
