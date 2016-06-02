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
    /// ������� ���������� CQRS
    /// ����� ����������������� ������� �������� � ������ � ��������� ��
    /// </summary>
    public class CqrsControllerBase : Controller
    {
        public static void ExcecuteCommand(object commandInstance)
        {
            // ����� ���������� ��� ������ �������
            var commandHandlerType =
                LoadAllAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .FirstOrDefault(i => i.Name == commandInstance.GetType().Name + "Handler");
            if (commandHandlerType == null)
                throw new ArgumentException("�� ������ ���������� " + commandInstance.GetType().Name + "Handler");

            // ������� ��������� ����������� ����� ���������, �.�. � ������������ ����� ���� �����������
            var commandHandlerInstance = UnityControllerFactory.ConfigContainer().Resolve(commandHandlerType);

            // ��������� �������
            var method = commandHandlerType.GetMethod("Handle");
            method.Invoke(commandHandlerInstance, new[] {commandInstance});
        }

        public static ICommand GetCommandInstance(string commandName, string json)
        {
            // �� ����� ������� ��������������� json � ������
            var commandType =
                LoadAllAssemblies().SelectMany(s => s.GetTypes())
                    .FirstOrDefault(i => i.Name == commandName);

            if (commandType == null)
                throw new ArgumentException("�� ������� �������� ������ ��� ������� " + commandName);

            var commandObject = JsonConvert.DeserializeObject(json, commandType);

            if (!(commandObject is ICommand))
                throw new ArgumentException("����� " + commandName + " �� ��������� ICommand");

            return commandObject as ICommand;
        }

        public static object ExecuteQuery(object queryInstance)
        {
            // ����� ���������� ��� ������� �������
            var queryHandlerType = LoadAllAssemblies().SelectMany(s => s.GetTypes())
                .FirstOrDefault(i => i.Name == queryInstance.GetType().Name + "Handler");
            if (queryHandlerType == null)
                throw new ArgumentException("�� ������ ���������� " + queryInstance.GetType().Name + "Handler");

            // ������� ��������� ����������� ����� ���������, �.�. � ������������ ����� ���� �����������
            var queryHandlerInstance = UnityControllerFactory.ConfigContainer().Resolve(queryHandlerType);

            // ��������� ������ � ������� ������
            var method = queryHandlerType.GetMethod("Handle");
            var result = method.Invoke(queryHandlerInstance, new[] {queryInstance});
            return result;
        }

        public static IQuery GetQueryInstance(string queryName, string json)
        {
            // �� ����� ������� ��������������� json � ������
            var queryType = LoadAllAssemblies().SelectMany(s => s.GetTypes())
                .FirstOrDefault(i => i.Name == queryName);

            if (queryType == null)
                throw new ArgumentException("�� ������� �������� ������ ��� ������� " + queryName);

            var queryInstance = JsonConvert.DeserializeObject(json, queryType);

            if (!(queryInstance is IQuery))
                throw new ArgumentException("����� " + queryName + " �� ��������� IAbstractQuery");

            return queryInstance as IQuery;
        }

        private static List<Assembly> _asseblies = null; 
        private static List<Assembly> LoadAllAssemblies()
        {
            if (_asseblies == null)
            {
                // ��������� ��� ������ �� ����������
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