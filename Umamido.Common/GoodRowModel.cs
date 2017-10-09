using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class GoodRowModel
    {
        public int GoodId { get; set; }

        public int RestaurantId { get; set; }

        public int RestaurantName { get; set; }

        public int ImageId { get; set; }

        public String FirstTitle { get; set; }
        public String FirstTitleNoHtml { get { return Tools.Tools.StripHtmlTags(FirstTitle); } }


        public String FirstDescription { get; set; }

        public Boolean IsActive { get; set; }

        public Decimal Price { get; set; }

        public Int32 CookTime { get; set; }

        public Int32 Similar1Id { get; set; }

        public Int32 Similar2Id { get; set; }

        public Int32 Similar3Id { get; set; }

        


        public TranslatableItemModel[] Titles { get; set; }

        public TranslatableItemModel[] Descriptions { get; set; }

    }
}
