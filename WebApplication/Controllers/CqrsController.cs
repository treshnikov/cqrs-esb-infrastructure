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
            // по имени команды десериализовать json в объект
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToList();
            var commandType = types.FirstOrDefault(i => i.Name == command);

            if (commandType == null)
                throw new ArgumentException("Не найдено описание класса для команды " + command);

            var commandObject = JsonConvert.DeserializeObject(args, commandType);

            // найти обработчик для данной команды
            var commandHandlerType = types.FirstOrDefault(i => i.Name == command + "Handler");
            if (commandType == null)
                throw new ArgumentException("Не найден обработчик " + command + "Handler");

            // создаем экземпляр обработчика через контейнер, т.к. в конструкторе могут быть зависимости
            var commandHandlerInstance = _container.Resolve(commandHandlerType);

            // выполнить команду
            var method = commandHandlerType.GetMethod("Handle");
            method.Invoke(commandHandlerInstance, new[] { commandObject });
        }

        public ActionResult Query(string service, string query, string args)
        {
            // по имени запроса десериализовать json в объект
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).ToList();
            var queryType = types.FirstOrDefault(i => i.Name == query);

            if (queryType == null)
                throw new ArgumentException("Не найдено описание класса для запроса " + query);

            var queryObject = JsonConvert.DeserializeObject(args, queryType);

            // найти обработчик для данного запроса
            var queryHandlerType = types.FirstOrDefault(i => i.Name == query + "Handler");
            if (queryType == null)
                throw new ArgumentException("Не найден обработчик " + query + "Handler");

            // создаем экземпляр обработчика через контейнер, т.к. в конструкторе могут быть зависимости
            var queryHandlerInstance = _container.Resolve(queryHandlerType);

            // выполнить запрос и вернуть данные
            var method = queryHandlerType.GetMethod("Handle");
            var result = method.Invoke(queryHandlerInstance, new[] { queryObject });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}