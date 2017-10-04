using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;

namespace Umamido.Site.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Index(int? invId)
        {
            InvModel model = new InvModel()
            {
                Address = "Цанко Церковски 1",
                EIK = "131198264",
                InvDate = DateTime.Now,
                InvNo = 122,
                IsOriginal = true,
                MOL="Александър Константинов",
                Receiver = "Информационни продукти ООД",
                VATNo = "BG131198264",
                Rows = new InvRowModel[] {
                    new InvRowModel(){ Article="Супа", Price=(decimal)10.22, Count=3},
                    new InvRowModel(){ Article="Бургер", Price=(decimal)12.22, Count=5},
                    new InvRowModel(){ Article="Паста", Price=(decimal)10.33, Count=2}
                }.ToList<InvRowModel>()
            };

            return View(model);
        }
    }
}