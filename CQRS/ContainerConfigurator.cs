using CQRS.DAL;
using Microsoft.Practices.Unity;

namespace CQRS
{
    public static class ContainerConfigurator
    {
        public static IUnityContainer Get()
        {
            var container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<IEsbMessageService, RabbitMqEsbMessageService>();
            container.RegisterType(typeof(IRepository), typeof(JsonRepository));

            return container;
        }
    }
}