using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Umamido.Admin.Controllers
{
    public class SharedController : BaseController
    {
        // GET: Shared
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Menu()
        {
            return PartialView(this.UserData);
        }
    }
}