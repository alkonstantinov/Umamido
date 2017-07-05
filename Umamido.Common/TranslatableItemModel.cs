using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class TranslatableItemModel
    {
        public int ID { get; set; }
        public int LangId { get; set; }
        public String LangName { get; set; }
        public String Text { get; set; }
    }
}
