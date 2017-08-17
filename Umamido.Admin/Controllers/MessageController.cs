using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umamido.Common;

namespace Umamido.Admin.Controllers
{
    public class MessageController : BaseController
    {
        // GET: Message
        public ActionResult Index(MessageBrowseModel model)
        {
            model.Dates = new SelectList(DL.GetMessageDates());
            if (model.Date == null && model.Dates.Count() > 0)
                model.Date = model.Dates.ToArray()[0].Text;
            model.Messages = DL.GetMessagesByDate(model.Date);
            return View(model);
        }
    }
}