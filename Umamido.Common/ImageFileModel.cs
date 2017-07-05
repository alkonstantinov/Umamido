using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class ImageFileModel
    {
        public String Guid { get; set; }

        public String FileName { get; set; }

        public Byte[] Content { get; set; }
    }
}
