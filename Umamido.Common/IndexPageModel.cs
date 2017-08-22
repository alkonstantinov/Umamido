using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class IndexPageModel
    {
        public SliderRowModel[] Sliders { get; set; }

        public String AboutUsMainHeading { get; set; }

        public String AboutUsMainText { get; set; }

        public RestaurantRowModel[] Restaurants { get; set; }

        public BlogRowModel[] Blogs { get; set; }
    }
}
