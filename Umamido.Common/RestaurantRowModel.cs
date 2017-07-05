using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class RestaurantRowModel
    {
        public int RestaurantId { get; set; }

        public int ImageId { get; set; }

        public String FirstTitle { get; set; }

        public Boolean IsActive { get; set; }

        public TranslatableItemModel[] Titles { get; set; }

        public TranslatableItemModel[] Descriptions { get; set; }

    }
}
