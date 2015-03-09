using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PcdWeb.Web.Models
{

    public class SendCode
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }
}