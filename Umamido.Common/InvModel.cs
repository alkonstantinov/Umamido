using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class InvModel
    {
        public String Receiver { get; set; }

        public String Address { get; set; }

        public String EIK { get; set; }

        public String VATNo { get; set; }

        public String MOL { get; set; }

        public int InvNo { get; set; }

        public DateTime InvDate { get; set; }

        public Boolean IsOriginal { get; set; }
        public List<InvRowModel> Rows { get; set; }

        public Decimal TotalNoVat
        {
            get
            {
                Decimal sum = 0;
                foreach (var r in Rows)
                    sum += r.Total;

                return sum;
            }
        }

        public Decimal Vat
        {
            get
            {
                Decimal sum = 0;
                foreach (var r in Rows)
                    sum += Math.Round((decimal)0.2 * r.Total, 2, MidpointRounding.AwayFromZero);
                return sum;
            }
        }

        public Decimal Total
        {
            get
            {
                return TotalNoVat + Vat;
            }
        }

        public String Slovom
        {
            get
            {
                return Tools.Tools.toSlovomLeva(Total);
            }
        }

    }
}
