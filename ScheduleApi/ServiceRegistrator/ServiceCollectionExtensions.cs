using System.Reflection;

namespace ScheduleApi.ServiceRegistrator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInjectables(this IServiceCollection services, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetExecutingAssembly();

            var typesWithAttribute = assembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.GetCustomAttribute<ServiceAttribute>() is not null
                );

            foreach (var implType in typesWithAttribute)
            {
                var attr = implType.GetCustomAttribute<ServiceAttribute>()!;
                var interfaces = implType.GetInterfaces();

                if (interfaces.Length == 0)
                {
                    // Без интерфейса — регаем сам класс
                    services.Add(new ServiceDescriptor(implType, implType, attr.Lifetime));
                }
                else
                {
                    foreach (var serviceType in interfaces)
                    {
                        services.Add(new ServiceDescriptor(serviceType, implType, attr.Lifetime));
                    }
                }
            }

            return services;
        }
    }
}