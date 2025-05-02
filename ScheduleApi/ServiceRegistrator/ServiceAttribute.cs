namespace ScheduleApi.ServiceRegistrator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; }

        public ServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Lifetime = lifetime;
        }
    }
}