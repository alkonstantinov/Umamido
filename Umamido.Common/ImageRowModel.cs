using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class ImageRowModel
    {
        public int ImageId { get; set; }
        public String ImageName { get; set; }
        public Boolean IsActive { get; set; }
        public String FileName { get; set; }
        public String Guid { get; set; }
        public byte[] Content { get; set; }
    }
}
