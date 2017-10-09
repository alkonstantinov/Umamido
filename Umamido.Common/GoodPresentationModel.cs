using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class GoodPresentationModel : GoodRowModel
    {
        public int? ClientId { get; set; }

        public Boolean CanOrder { get; set; }

        public int Count { get; set; }

        public RestaurantRowModel[] AllRestaurants { get; set; }

        public GoodRowModel[] SimilarGoods { get; set; }

    }
}
