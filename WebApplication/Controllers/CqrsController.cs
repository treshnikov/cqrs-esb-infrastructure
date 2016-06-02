using System.Reflection;
using System.Web.Mvc;
using CQRS;
using Microsoft.Practices.Unity;
using UsersService;

namespace WebApplication.Controllers
{
    public class CqrsController : CqrsControllerBase
    {
        public CqrsController(IUnityContainer container) : base()
        {
        }

        public void Command(string commandName, string json)
        {
            var commandInstance = GetCommandInstance(commandName, json);
            ExcecuteCommand(commandInstance);
        }

        public ActionResult Query(string queryName, string json)
        {
            var queryInstance = GetQueryInstance(queryName, json);
            var result = ExecuteQuery(queryInstance);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}