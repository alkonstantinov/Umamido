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
                if (result.HasValue)
                {
                    this.UserId = result.Value;
                    if (model.ReturnUrl != null)
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("index", "home");
                }
                model.LoginFailure = true;
            }
            return View(model);
        }

        #region Users
        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AllUsers()
        {
            var result = this.DL.GetUsers();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UserChangeActive(int userId)
        {
            this.DL.UserChangeActive(userId);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetUser(UserRowModel model)
        {
            this.DL.SaveUser(model);
            return Json("ОК", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetUser(int userId)
        {
            return Json(this.DL.GetUser(userId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UserExists(string userName)
        {
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