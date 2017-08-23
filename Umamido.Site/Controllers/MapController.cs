using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;

namespace Umamido.Site.Controllers
{
    public class MapController : BaseController
    {
        // GET: Map
        [HttpPost]
        public ActionResult CheckAddress(string address)
        {
            var dbResult = DL.CheckAddress(address);
            if (dbResult == null)
            {
                List<AddressCheckModel> result = new List<AddressCheckModel>();
                result.Add(new AddressCheckModel() { Address = "Адрес 1", Km = 3, IsIn = true });
                result.Add(new AddressCheckModel() { Address = "Адрес 2", Km = 2, IsIn = true });
                result.Add(new AddressCheckModel() { Address = "Адрес 3", Km = 8, IsIn = false });
                foreach (var item in result)
                    DL.AddAddressCheck(item);
                return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            }

            dbResult.IsIn = dbResult.Km <= 3;
            return Json(new AddressCheckModel[] { dbResult}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveAddressInSession(string address)
        {
            Address = address;
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

    }
}