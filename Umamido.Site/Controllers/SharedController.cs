using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Umamido.Site.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult SetLanguage(string language)
        {
            HttpContext.Session["culture"] = language;
            return Redirect(Request.UrlReferrer.AbsoluteUri);

        }
    }
}