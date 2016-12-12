using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            // 
            return Redirect("~/app/#/home");
        }

        public ActionResult UnauthorisedException(string message)
        {
            // в случае нехватки прав - редирект на главную страницу с очисткой прав
            return Redirect("~/app/#/home");
        }
    }
}