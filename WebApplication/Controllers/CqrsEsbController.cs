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
        private readonly IMessageService _messageService;

        public CqrsEsbController(IUnityContainer container, IMessageService messageService) : base(container)
        {
            _messageService = messageService;
        }

        public void Command(string serviceName, string commandName, string json)
        {
            var commandObject = GetCommandInstance(commandName, json);
            
            // отправить команду в шину данных
            _messageService.SendCommandAsync(
                new CommandMessage(JsonConvert.SerializeObject(commandObject), 
                commandObject.ServiceName));
        }

        public ActionResult Query(string serviceName, string queryName, string json)
        {
            var queryInstance = GetQueryInstance(queryName, json);

            // отправить запрос, получить ответ
            var res = _messageService.SendQueryAsync(
                new QueryMessage(JsonConvert.SerializeObject(queryInstance), 
                queryInstance.ServiceName)).Result;

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}