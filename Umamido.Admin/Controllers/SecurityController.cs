using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;

namespace Umamido.Admin.Controllers
{
    public class SecurityController : BaseController
    {
        // GET: Security
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(LoginModel model)
        {
            if (this.Request.HttpMethod.ToUpper() == "GET")
            {
                model = new LoginModel();
                model.ReturnUrl = this.Request.QueryString["ReturnUrl"];

            }
            else
            {
                var result = this.DL.Login(model);
                if (result != null)
                {
                    this.UserData = result;
                    if (model.ReturnUrl != null)
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("index", "home");
                }
                model.LoginFailure = true;
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult Logoff()
        {
            Session.Clear();
            Session.Abandon();
            return View("Index", new LoginModel());
        }
        #region Users
        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AllUsers(int userLevelId = -1)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);
            var result = this.DL.GetUsers(userLevelId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserLevels()
        {
            var result = this.DL.GetUserLevels();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UserChangeActive(int userId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.UserChangeActive(userId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetUser(UserRowModel model)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            this.DL.SaveUser(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUser(int userId)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            return Json(this.DL.GetUser(userId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UserExists(string userName)
        {
            if (!HasLevel(1))
                return Json("-1", JsonRequestBehavior.AllowGet);

            return Json(this.DL.UserExists(userName), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ErrorJS(ErrorJSModel model)
        {
            return View(model);
        }

        #endregion
    }
}