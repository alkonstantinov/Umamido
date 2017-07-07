using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class ReqQueryRowModel
    {
        public int ReqId { get; set; }
        public DateTime OnDate { get; set; }
        public String Receiver { get; set; }
        public String Address { get; set; }
        public String PaymentTypeName { get; set; }
        public String StatusName { get; set; }
        public Decimal Total { get; set; }
        public Boolean Paid { get; set; }
        public String Note { get; set; }
    }
}
