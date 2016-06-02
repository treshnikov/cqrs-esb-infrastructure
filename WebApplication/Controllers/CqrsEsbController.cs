using System;
using System.Linq;
using System.Web.Mvc;
using CQRS;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApplication.Controllers
{
    public class CqrsEsbController : CqrsControllerBase
    {
        private readonly IEsbMessageService _esbMessageService;

        public CqrsEsbController(IUnityContainer container, IEsbMessageService esbMessageService) : base()
        {
            _esbMessageService = esbMessageService;
        }

        public void Command(string commandName, string json)
        {
            var commandObject = GetCommandInstance(commandName, json);
            
            // отправить команду в шину данных
            _esbMessageService.Send(
                new EsbMessage(
                    commandObject.ServiceName,
                    commandName,
                    JsonConvert.SerializeObject(commandObject)
                ));
        }

        public ActionResult Query(string queryName, string json)
        {
            var queryInstance = GetQueryInstance(queryName, json);

            // отправить запрос, получить ответ
            var res = _esbMessageService.SendAndGetResult(
                new EsbMessage(
                    queryInstance.ServiceName,
                    queryName,
                    JsonConvert.SerializeObject(queryInstance)
                ));

            if (res.IsError)
                throw new Exception(res.ErrorText);

            return Content(res.Body, "application/json");
            
        }
    }
}