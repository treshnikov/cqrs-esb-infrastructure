using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    public class CqrsEsbController : CqrsControllerBase
    {
        public CqrsEsbController(IUnityContainer container) : base(container)
        {
        }

        public void Command(string serviceName, string commandName, string json)
        {
            var commandObject = GetCommandInstance(commandName, json);

            // ��������� ������� � ���� ������
        }

        public ActionResult Query(string serviceName, string queryName, string json)
        {
            var queryInstance = GetQueryInstance(queryName, json);

            // ��������� ������, �������� �����.

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}