using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using CQRS;
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
        private  readonly IUnityContainer _container;

        public UnityControllerFactory()
        {
            _container = new UnityContainer();
            _container.RegisterInstance<IUnityContainer>(_container);

            //todo
            //var queryHandlers = AppDomain.CurrentDomain.GetAssemblies()
            //    .SelectMany(s => s.GetTypes())
            //    .Where(p => !p.IsInterface && typeof(IQueryHandler<,>).IsAssignableFrom(p)).ToList();
            //foreach (var queryHandler in queryHandlers)
            //{
            //    var queryType = queryHandler.GetGenericArguments()[0];
            //    var queryResultType = queryHandler.GetGenericArguments()[1];
            //    var t = queryHandler.GetInterface("IQueryHandler");
            //    _container.RegisterType(t, queryHandler);
            //}
        }

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            var controllerType = GetControllerType(requestContext, controllerName);
            return _container.Resolve(controllerType) as IController;
        }
    }
}
