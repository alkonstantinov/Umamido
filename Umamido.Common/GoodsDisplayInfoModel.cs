using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class GoodsDisplayInfoModel
    {
        public int RestaurantId { get; set; }

        public bool ShowAllRestaurants { get; set; }

        public bool IsLogged { get; set; }


        public RestaurantRowModel[] Restaurants { get; set; }
    }
}
