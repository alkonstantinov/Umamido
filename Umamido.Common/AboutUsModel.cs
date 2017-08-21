using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class AboutUsModel
    {
        public RestaurantRowModel[] Restaurants { get; set; }

        public String Title { get; set; }

        public String Text { get; set; }
    }
}
