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
            
            // отправить команду в шину данных
            _esbMessageService.Send(
                new EsbMessage(JsonConvert.SerializeObject(commandObject), 
                commandObject.ServiceName));
        }

        public string Query(string queryName, string json)
        {
            var queryInstance = GetQueryInstance(queryName, json);

            // отправить запрос, получить ответ
            var res = _esbMessageService.SendAndGetResult(
                new EsbMessage(JsonConvert.SerializeObject(queryInstance), 
                queryInstance.ServiceName)).Result;

            if (res.IsError)
                throw new Exception(res.ErrorText);

            return res.Body;
        }
    }
}