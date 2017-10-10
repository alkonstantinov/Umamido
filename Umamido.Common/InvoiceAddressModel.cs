using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class InvoiceAddressModel
    {
        public int ClientId { get; set; }
        public String CompanyName { get; set; }
        public String EIK { get; set; }
        public String VAT { get; set; }
        public String PersonName { get; set; }
        public String CompanyAddress { get; set; }
        public String Country{ get; set; }
        public String PK { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }

        public String ErrorMessage { get; set; }


    }
}
