using System;
using System.Collections.Generic;

namespace Suteki.Shop.ViewData
{
    public class HowDidYouHearOfUsViewModel
    {
        public HowDidYouHearOfUsViewModel()
        {
            Lines = new List<HowDidYouHearOfUsLine>();
        }

        public IList<HowDidYouHearOfUsLine> Lines { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class HowDidYouHearOfUsLine
    {
        public string Option { get; set; }
        public int Count { get; set; }
    }
}