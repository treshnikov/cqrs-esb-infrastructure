using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendCommand(string arg)
        {
            return Json("[x] ok - " + arg, JsonRequestBehavior.AllowGet);
        }
    }
}