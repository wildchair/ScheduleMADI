namespace ScheduleApi.ServiceRegistrator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InjectableAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; }

        public InjectableAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Lifetime = lifetime;
        }
    }
}
