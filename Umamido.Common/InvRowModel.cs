using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class InvRowModel
    {
        public String Article { get; set; }

        public Decimal Price { get; set; }

        public Int32 Count { get; set; }
        
        public Decimal Total {
            get {
                return Price * Count;
            }
        }
    }
}
