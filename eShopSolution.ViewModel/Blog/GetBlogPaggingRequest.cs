using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Blog
{
    public class GetBlogPaggingRequest : PaggingRequestBase
    {
        public string Keywork { set; get; }
    }
}
