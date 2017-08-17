using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class MessageModel
    {
        public String FromName { get; set; }
        public String EMail { get; set; }
        public String Subject { get; set; }
        public String MessageText { get; set; }
        public DateTime OnDate { get { return DateTime.Now; } }
    }
}
