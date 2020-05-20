using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products
{
    public class GetProductPaggingRequest : PaggingRequestBase
    {
        public int? CategoryId { set; get; }
        public string LanguageId { set; get; }
        public string Category { set; get; }
        public string Keyword { set; get; }
        public int MinPrice { set; get; }
        public int MaxPrice { set; get; }
        public string CategoryUrl { set; get; }

    }
}
