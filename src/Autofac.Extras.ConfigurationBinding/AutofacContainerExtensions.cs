using Autofac.Builder;

namespace Autofac.Extras.ConfigurationBinding
{
    public static class AutofacContainerExtensions
    {
        public static IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> RegisterConfiguration<T>(this ContainerBuilder builder)
            where T : class
        {
            var proxyObject = ConfigurationBinder.CreateDynamicProxy<T>();
            return builder.Register(context => proxyObject).As<T>();
        }
    }
}