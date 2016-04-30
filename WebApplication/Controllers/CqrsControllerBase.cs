using System;
using System.Linq;
using System.Web.Mvc;
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
        private readonly IUnityContainer _container;

        public CqrsControllerBase(IUnityContainer container)
        {
            _container = container;
        }

        protected void ExcecuteCommand(object commandInstance)
        {
            // ����� ���������� ��� ������ �������
            var commandHandlerType =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .FirstOrDefault(i => i.Name == commandInstance.GetType().Name + "Handler");
            if (commandHandlerType == null)
                throw new ArgumentException("�� ������ ���������� " + commandInstance.GetType().Name + "Handler");

            // ������� ��������� ����������� ����� ���������, �.�. � ������������ ����� ���� �����������
            var commandHandlerInstance = UnityContainerExtensions.Resolve(_container, commandHandlerType);

            // ��������� �������
            var method = commandHandlerType.GetMethod("Handle");
            method.Invoke(commandHandlerInstance, new[] {commandInstance});
        }

        protected static object GetCommandInstance(string commandName, string json)
        {
            // �� ����� ������� ��������������� json � ������
            var commandType =
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                    .FirstOrDefault(i => i.Name == commandName);

            if (commandType == null)
                throw new ArgumentException("�� ������� �������� ������ ��� ������� " + commandName);

            var commandObject = JsonConvert.DeserializeObject(json, commandType);
            return commandObject;
        }

        protected object ExecuteQuery(object queryInstance)
        {
            // ����� ���������� ��� ������� �������
            var queryHandlerType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .FirstOrDefault(i => i.Name == queryInstance.GetType().Name + "Handler");
            if (queryHandlerType == null)
                throw new ArgumentException("�� ������ ���������� " + queryInstance.GetType().Name + "Handler");

            // ������� ��������� ����������� ����� ���������, �.�. � ������������ ����� ���� �����������
            var queryHandlerInstance = UnityContainerExtensions.Resolve(_container, queryHandlerType);

            // ��������� ������ � ������� ������
            var method = queryHandlerType.GetMethod("Handle");
            var result = method.Invoke(queryHandlerInstance, new[] {queryInstance});
            return result;
        }

        protected static object GetQueryInstance(string queryName, string json)
        {
            // �� ����� ������� ��������������� json � ������
            var queryType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .FirstOrDefault(i => i.Name == queryName);

            if (queryType == null)
                throw new ArgumentException("�� ������� �������� ������ ��� ������� " + queryName);

            var queryInstance = JsonConvert.DeserializeObject(json, queryType);
            return queryInstance;
        }
    }
}