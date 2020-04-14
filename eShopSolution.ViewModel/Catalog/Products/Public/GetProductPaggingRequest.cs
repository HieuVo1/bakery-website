using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products.Public
{
    public class GetProductPaggingRequest : PaggingRequestBase
    {
        public int? CategoryId { set; get; }
        public string Keywork { set; get; }

    }
}
