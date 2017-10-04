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

        public ClientData ClientData
        {
            get
            {
                if (Session["ClientData"] == null)
                    Session["ClientData"] = new ClientData();

                return (ClientData)Session["ClientData"];
            }
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

        public string Address
        {
            get
            {
                return (Session["Address"] == null ? null : Address);
            }
            set
            {
                Session["Address"] = value;
            }
        }


        public BaseController()
        {
            this.DL = new Umamido.DL.DLFuncs();
        }

        
    }
}