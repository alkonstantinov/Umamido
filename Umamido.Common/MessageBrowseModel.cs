using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Umamido.Common
{
    public class MessageBrowseModel
    {
        public SelectList Dates { get; set; }
        public String Date { get; set; }
        public MessageModel[] Messages { get; set; }

    }
}
