using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class MyProfileModel
    {
        public OrderInfoModel LastOrder { get; set; }

        public OrderInfoModel[] PrevOrders { get; set; }



    }
}
