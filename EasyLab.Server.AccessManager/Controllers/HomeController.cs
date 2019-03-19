using EasyLab.Server.AccessManager.Models;
using System.Web.Mvc;

namespace EasyLab.Server.AccessManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "EasyLab Server Test Program";
            ViewBag.Address = GlobalSettings.M1RESTServiceBaseAddress;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Detail()
        {
            ViewBag.Message = "Your Detail page.";

            return View();
        }
    }
}
