using System.Web.Mvc;

namespace TestGroup.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}