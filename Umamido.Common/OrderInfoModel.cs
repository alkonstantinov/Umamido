using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class OrderInfoModel
    {
        public int OrderNum { get; set; }

        public DateTime OrderDate { get; set; }

        public String OrderProducts { get; set; }

        public decimal Total { get; set; }

        public int OrderStatus { get; set; }

    }
}
