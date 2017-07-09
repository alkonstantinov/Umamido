using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;

namespace Umamido.Admin.Controllers
{
    public class DispatchController : BaseController
    {
        // GET: Dispatch
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AllForDispatch()
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);
            
            var result = this.DL.GetReqsForDispatch();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetDispatch(DispatchModel model)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.Dispatch(model);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

    }
}