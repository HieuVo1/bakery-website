using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Categories
{
    public class GetCategoryPaggingReqest: PaggingRequestBase
    {
        public string Keywork { set; get; }
    }
}
