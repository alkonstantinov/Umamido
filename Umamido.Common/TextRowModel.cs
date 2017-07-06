using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class TextRowModel
    {
        public int TextId { get; set; }
        
        public String TextName { get; set; }

        
        public TranslatableItemModel[] Descriptions { get; set; }

    }
}
