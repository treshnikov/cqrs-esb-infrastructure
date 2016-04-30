using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Базовый контроллер CQRS
    /// Умеет десериализовывать объекты запросов и команд и выполнять их
    /// </summary>
    public class CqrsControllerBase : Controller
    {
        private readonly IUnityContainer _container;

        public CqrsControllerBase(IUnityContainer container)
        {
            _container = container;
        }

        protected void ExcecuteCommand(object commandInstance)
        {
            // найти обработчик для данной команды
            var commandHandlerType =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .FirstOrDefault(i => i.Name == commandInstance.GetType().Name + "Handler");
            if (commandHandlerType == null)
                throw new ArgumentException("Не найден обработчик " + commandInstance.GetType().Name + "Handler");

            // создаем экземпляр обработчика через контейнер, т.к. в конструкторе могут быть зависимости
            var commandHandlerInstance = UnityContainerExtensions.Resolve(_container, commandHandlerType);

            // выполнить команду
            var method = commandHandlerType.GetMethod("Handle");
            method.Invoke(commandHandlerInstance, new[] {commandInstance});
        }

        protected static object GetCommandInstance(string commandName, string json)
        {
            // по имени команды десериализовать json в объект
            var commandType =
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                    .FirstOrDefault(i => i.Name == commandName);

            if (commandType == null)
                throw new ArgumentException("Не найдено описание класса для команды " + commandName);

            var commandObject = JsonConvert.DeserializeObject(json, commandType);
            return commandObject;
        }

        protected object ExecuteQuery(object queryInstance)
        {
            // найти обработчик для данного запроса
            var queryHandlerType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .FirstOrDefault(i => i.Name == queryInstance.GetType().Name + "Handler");
            if (queryHandlerType == null)
                throw new ArgumentException("Не найден обработчик " + queryInstance.GetType().Name + "Handler");

            // создаем экземпляр обработчика через контейнер, т.к. в конструкторе могут быть зависимости
            var queryHandlerInstance = UnityContainerExtensions.Resolve(_container, queryHandlerType);

            // выполнить запрос и вернуть данные
            var method = queryHandlerType.GetMethod("Handle");
            var result = method.Invoke(queryHandlerInstance, new[] {queryInstance});
            return result;
        }

        protected static object GetQueryInstance(string queryName, string json)
        {
            // по имени запроса десериализовать json в объект
            var queryType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .FirstOrDefault(i => i.Name == queryName);

            if (queryType == null)
                throw new ArgumentException("Не найдено описание класса для запроса " + queryName);

            var queryInstance = JsonConvert.DeserializeObject(json, queryType);
            return queryInstance;
        }
    }
}