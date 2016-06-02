using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using CQRS;
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
        public static void ExcecuteCommand(object commandInstance)
        {
            // найти обработчик для данной команды
            var commandHandlerType =
                LoadAllAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .FirstOrDefault(i => i.Name == commandInstance.GetType().Name + "Handler");
            if (commandHandlerType == null)
                throw new ArgumentException("Не найден обработчик " + commandInstance.GetType().Name + "Handler");

            // создаем экземпляр обработчика через контейнер, т.к. в конструкторе могут быть зависимости
            var commandHandlerInstance = UnityControllerFactory.ConfigContainer().Resolve(commandHandlerType);

            // выполнить команду
            var method = commandHandlerType.GetMethod("Handle");
            method.Invoke(commandHandlerInstance, new[] {commandInstance});
        }

        public static ICommand GetCommandInstance(string commandName, string json)
        {
            // по имени команды десериализовать json в объект
            var commandType =
                LoadAllAssemblies().SelectMany(s => s.GetTypes())
                    .FirstOrDefault(i => i.Name == commandName);

            if (commandType == null)
                throw new ArgumentException("Не найдено описание класса для команды " + commandName);

            var commandObject = JsonConvert.DeserializeObject(json, commandType);

            if (!(commandObject is ICommand))
                throw new ArgumentException("Класс " + commandName + " не реализует ICommand");

            return commandObject as ICommand;
        }

        public static object ExecuteQuery(object queryInstance)
        {
            // найти обработчик для данного запроса
            var queryHandlerType = LoadAllAssemblies().SelectMany(s => s.GetTypes())
                .FirstOrDefault(i => i.Name == queryInstance.GetType().Name + "Handler");
            if (queryHandlerType == null)
                throw new ArgumentException("Не найден обработчик " + queryInstance.GetType().Name + "Handler");

            // создаем экземпляр обработчика через контейнер, т.к. в конструкторе могут быть зависимости
            var queryHandlerInstance = UnityControllerFactory.ConfigContainer().Resolve(queryHandlerType);

            // выполнить запрос и вернуть данные
            var method = queryHandlerType.GetMethod("Handle");
            var result = method.Invoke(queryHandlerInstance, new[] {queryInstance});
            return result;
        }

        public static IQuery GetQueryInstance(string queryName, string json)
        {
            // по имени запроса десериализовать json в объект
            var queryType = LoadAllAssemblies().SelectMany(s => s.GetTypes())
                .FirstOrDefault(i => i.Name == queryName);

            if (queryType == null)
                throw new ArgumentException("Не найдено описание класса для запроса " + queryName);

            var queryInstance = JsonConvert.DeserializeObject(json, queryType);

            if (!(queryInstance is IQuery))
                throw new ArgumentException("Класс " + queryName + " не реализует IAbstractQuery");

            return queryInstance as IQuery;
        }

        private static List<Assembly> _asseblies = null; 
        private static List<Assembly> LoadAllAssemblies()
        {
            if (_asseblies == null)
            {
                // подгружаю все сборки из директории
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
                var binPath =
                    Path.GetDirectoryName(
                        Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
                var documentsAssemblies = Directory.GetFiles(binPath + "\\", "*.dll");
                loadedAssemblies.AddRange(
                    documentsAssemblies.Select(
                        documentsAssembly =>
                            AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(documentsAssembly))));
                _asseblies = loadedAssemblies;
            }
            return _asseblies;
        }
    }
}