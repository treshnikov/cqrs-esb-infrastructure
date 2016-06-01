using System;
using System.Linq;
using System.Web.Mvc;
using CQRS;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    public class CqrsEsbController : CqrsControllerBase
    {
        private readonly IEsbMessageService _esbMessageService;

        public CqrsEsbController(IUnityContainer container, IEsbMessageService esbMessageService) : base(container)
        {
            _esbMessageService = esbMessageService;
        }

        public void Command(string commandName, string json)
        {
            var commandObject = GetCommandInstance(commandName, json);
            
            // ��������� ������� � ���� ������
            _esbMessageService.Send(
                new EsbMessage(
                    commandObject.ServiceName,
                    commandName,
                    JsonConvert.SerializeObject(commandObject)
                ));
        }

        public string Query(string queryName, string json)
        {
            var queryInstance = GetQueryInstance(queryName, json);

            // ��������� ������, �������� �����
            var res = _esbMessageService.SendAndGetResult(
                new EsbMessage(
                    queryInstance.ServiceName,
                    queryName,
                    JsonConvert.SerializeObject(queryInstance)
                )).Result;

            if (res.IsError)
                throw new Exception(res.ErrorText);

            return res.Body;
        }
    }
}