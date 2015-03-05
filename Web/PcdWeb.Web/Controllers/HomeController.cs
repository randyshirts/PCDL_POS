using System.Web.Mvc;

namespace PcdWeb.Web.Controllers
{
    public class HomeController : PcdWebControllerBase
    {
        public ActionResult Index()
        {
            return View("~/App/Main/shell.cshtml"); //Layout of the durandal application.
        }
	}
}