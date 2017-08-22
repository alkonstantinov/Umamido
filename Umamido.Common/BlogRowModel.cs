using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umamido.Common
{
    public class BlogRowModel
    {
        public int BlogId { get; set; }
        public int PrevId { get; set; }
        public int NextId { get; set; }


        public DateTime OnDate { get; set; }

        public int ImageId { get; set; }

        public String FirstTitle { get; set; }
        public String FirstTitleNoHtml { get { return Tools.Tools.StripHtmlTags(FirstTitle); } }


        public String FirstDescription { get; set; }
        public String FirstDescriptionNoHtml { get { return Tools.Tools.StripHtmlTags(FirstDescription); } }
        public String FirstDescriptionShort
        {
            get
            {
                if (FirstDescription == null)
                    return null;
                string result = FirstDescriptionNoHtml;
                int last = 200;
                if (result.Length < last)
                    return result;
                while (last > -1 && result[last] != ' ')
                    last--;
                if (last == -1)
                    return result.Substring(0, 200);
                else
                    return result.Substring(0, last);
            }
        }

        public Boolean IsActive { get; set; }


        public TranslatableItemModel[] Titles { get; set; }

        public TranslatableItemModel[] Descriptions { get; set; }

    }
}
