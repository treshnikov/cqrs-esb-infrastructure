using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using CQRS;
using CQRS.DAL;
using Microsoft.Practices.Unity;

namespace WebApplication
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            IControllerFactory unityControllerFactory = new UnityControllerFactory();
            ControllerBuilder.Current.SetControllerFactory(unityControllerFactory);

        }

    }

    public class UnityControllerFactory : DefaultControllerFactory
    {
        private IUnityContainer _container;

        public UnityControllerFactory()
        {
            _container = ConfigContainer();
        }

        public static IUnityContainer ConfigContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterType<IEsbMessageService, RabbitMqEsbMessageService>();
            container.RegisterType(typeof (IRepository), typeof (JsonRepository));

            return container;
        }

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            var controllerType = GetControllerType(requestContext, controllerName);
            return _container.Resolve(controllerType) as IController;
        }
    }
}
