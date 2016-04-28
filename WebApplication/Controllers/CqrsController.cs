using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using CQRS;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using UsersService;

namespace WebApplication.Controllers
{
    public class CqrsController : Controller
    {
        private readonly IUnityContainer _container;

        public CqrsController(IUnityContainer container)
        {
            _container = container;
        }

        public void Command(string service, string command, string args)
        {
        }

        public ActionResult Query(string service, string query, string args)
        {
            // todo - добавить контейнер

            // по имени запроса десериализовать json в объект
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToList();
            var queryType = types.FirstOrDefault(i => i.Name == query);

            if (queryType == null)
                throw new ArgumentException(query);

            var queryObject = JsonConvert.DeserializeObject(args, queryType);

            // найти обработчик для данного запроса
            var queryHandlerType = types.FirstOrDefault(i => i.Name == query + "Handler");
            var queryHandlerInstance = Activator.CreateInstance(queryHandlerType);

            // выполнить запрос и вернуть данные
            var method = queryHandlerType.GetMethod("Handle");
            var result = method.Invoke(queryHandlerInstance, new object[1] { queryObject });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}