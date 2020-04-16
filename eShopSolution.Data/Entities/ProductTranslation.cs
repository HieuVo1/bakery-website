using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ProductTranslation
    {
        public int ProductId { set; get; }
        public Product Product { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string ProductUrl { set; get; }
        public int LanguageId { set; get; }
        public Language Language { set; get; }
    }
}
