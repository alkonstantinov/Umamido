using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class AddressModel
    {
        public int ClientId { get; set; }
        public int AddressNum { get; set; }
        public String Name { get; set; }
        public String Family { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public String[] AddressVariants { get; set; }
        public String ErrorMessage { get; set; }

    }
}
