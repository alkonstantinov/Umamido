using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class DispatchModel
    {
        public int ReqId { get; set; }

        public int UserId { get; set; }

        public int NewStatus { get; set; }

        public string Note { get; set; }
    }
}
