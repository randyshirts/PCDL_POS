using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PcdWeb.Web.Controllers
{
    public class WidgetsController : PcdWebControllerBase
    {
        public ActionResult LoginBox()
        {
            return PartialView("~/Widgets/LoginBox/LoginBox.cshtml");
        }
    }
}