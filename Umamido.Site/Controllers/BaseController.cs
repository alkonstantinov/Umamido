using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;

namespace Umamido.Site.Controllers
{
    public class BaseController : Controller
    {
        public DL.DLFuncs DL { get; set; }

        public UserRowModel UserData
        {
            get { return (Session["UserId"] == null ? null : (UserRowModel)Session["UserId"]); }
            set { Session["UserId"] = value; }
        }




        public string Lang
        {
            get
            {
                return (Session["culture"] == null ? "bg-BG" : Session["culture"].ToString());
            }
            set
            {
                Session["culture"] = value;
            }
        }

        public BaseController()
        {
            this.DL = new Umamido.DL.DLFuncs();
        }

        public bool HasLevel(int level)
        {
            return this.UserData != null && this.UserData.UserLevelId == level;
        }
    }
}