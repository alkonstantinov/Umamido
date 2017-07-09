using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;
using Umamido.Common.Tools;

namespace Umamido.Admin.Controllers
{
    public class ReportsController : BaseController
    {
        // GET: Reports
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]

        public ActionResult RequestsQuery(ReqQueryModel model)
        {
            if (this.UserData.UserLevelId != 1)
                return RedirectToAction("logoff", "security");
            if (this.Request.HttpMethod.ToUpper() == "GET")
            {
                model.Restaurant = -1;
                model.SelectedTimeFrame = 1;
            }
            model.Restaurants = new SelectList(DL.GetRestaurants(), "RestaurantId", "FirstTitleNoHtml");
            model.TimeFrame = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem { Text="Day", Value = "1"},
                new SelectListItem { Text="Week", Value = "2"},
                new SelectListItem { Text="Month", Value = "3"},
                new SelectListItem { Text="Year", Value = "4"},
                new SelectListItem { Text="2 Years", Value = "5"}
            }, "Value", "Text");
            if (model.Restaurant == 0)
                model.Restaurant = -1;
            DL.ReqQuery(model);
            return View(model);
        }
    }
}