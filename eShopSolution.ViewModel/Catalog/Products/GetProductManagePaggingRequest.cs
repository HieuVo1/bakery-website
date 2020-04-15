using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class GetProductManagePaggingRequest : PaggingRequestBase
    {
        public List<int> CategoryIds { set; get; }
        public string Keywork { set; get; }

    }
}
