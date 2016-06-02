using System.Reflection;
using System.Web.Mvc;
using CQRS;
using Microsoft.Practices.Unity;
using UsersService;

namespace WebApplication.Controllers
{
    public class CqrsController : Controller
    {
        public CqrsController(IUnityContainer container) : base()
        {
        }

        public void Command(string commandName, string json)
        {
            var commandInstance = CqrsHelper.GetCommandInstance(commandName, json);
            CqrsHelper.ExcecuteCommand(commandInstance);
        }

        public ActionResult Query(string queryName, string json)
        {
            var queryInstance = CqrsHelper.GetQueryInstance(queryName, json);
            var result = CqrsHelper.ExecuteQuery(queryInstance);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}