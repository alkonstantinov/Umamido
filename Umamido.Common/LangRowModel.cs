using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class LangRowModel
    {
        public int LangId { get; set; }

        public String LangName { get; set; }
        public Boolean IsActive { get; set; }
    }
}
