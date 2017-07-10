using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;
using Umamido.Common.Tools;

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

        [HttpGet]
        public ActionResult ForCollect()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AllForCollect()
        {
            if (!HasLevel(2))
                return Json("-1", JsonRequestBehavior.AllowGet);
            return Json(DL.GetReqsForCollect(this.UserData.UserId), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CollectDetails(int reqId)
        {
            if (!HasLevel(2))
                return Json("-1", JsonRequestBehavior.AllowGet);
            var result = DL.CollectDetails(reqId);
            foreach (var r in result)
            {
                r.Restaurant = Tools.StripHtmlTags(r.Restaurant);
                r.Good = Tools.StripHtmlTags(r.Good);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetCollected(DispatchModel model)
        {
            if (!HasLevel(2))
                return Json("-1", JsonRequestBehavior.AllowGet);

            model.UserId = this.UserData.UserId;
            this.DL.Collected(model);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult ForDeliver()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AllForDeliver()
        {
            if (!HasLevel(2))
                return Json("-1", JsonRequestBehavior.AllowGet);
            return Json(DL.GetReqsForDelivery(this.UserData.UserId), JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public ActionResult SetDelivered(DispatchModel model)
        {
            if (!HasLevel(2))
                return Json("-1", JsonRequestBehavior.AllowGet);

            model.UserId = this.UserData.UserId;
            this.DL.Delivered(model);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }


    }
}