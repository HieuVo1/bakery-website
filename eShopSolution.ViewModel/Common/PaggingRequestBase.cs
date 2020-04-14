using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Common
{
    public class PaggingRequestBase
    {
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
    }
}
