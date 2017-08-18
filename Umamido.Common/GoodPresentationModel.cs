using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class GoodPresentationModel:GoodRowModel
    {
        public RestaurantRowModel[] AllRestaurants { get; set; }

        public GoodRowModel[] SimilarGoods { get; set; }

    }
}
