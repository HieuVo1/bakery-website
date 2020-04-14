using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products.Manage
{
    public class GetProductPaggingRequest : PaggingRequestBase
    {
        public string Keywork { set; get; }
        public List<int> CategoryIds { set; get; }
    }
}
