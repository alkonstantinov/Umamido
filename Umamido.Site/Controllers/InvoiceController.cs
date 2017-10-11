using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;

namespace Umamido.Site.Controllers
{
    public class InvoiceController : BaseController
    {
        // GET: Invoice
        public ActionResult Index(int? invId, bool isOriginal)
        {



            InvModel model = DL.GetInvoiceFromReq(invId.Value);
            model.Rows.Add(
                new InvRowModel()
                {
                    Article = "Доставка",
                    Count = 1,
                    Price = decimal.Parse(ConfigurationManager.AppSettings["DeliveryPrice"]) / (decimal)1.2
                }
                );
            model.IsOriginal = isOriginal;

            return View(model);
        }
    }
}