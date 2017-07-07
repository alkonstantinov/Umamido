using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Umamido.Common
{
    public class ReqQueryModel
    {
        public int SelectedTimeFrame { get; set; }
        public SelectList TimeFrame { get; set; }
        public int Restaurant { get; set; }
        public SelectList Restaurants { get; set; }
        public ReqQueryRowModel[] Result { get; set; }
    }
}
