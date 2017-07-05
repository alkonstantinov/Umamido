using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Umamido.Admin.Controllers
{
    public class BaseController : Controller
    {
        public DL.DLFuncs DL { get; set; }

        public int? UserId
        {
            get { return (Session["UserId"] == null ? null : (int?)int.Parse(Session["UserId"].ToString())); }
            set { Session["UserId"] = value; }
        }


        public BaseController()
        {
            this.DL = new Umamido.DL.DLFuncs();
        }
    }
}