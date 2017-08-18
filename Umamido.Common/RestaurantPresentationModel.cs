using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class RestaurantPresentationModel: RestaurantRowModel
    {
        public RestaurantRowModel[] AllRestaurants { get; set; }
    }
}
