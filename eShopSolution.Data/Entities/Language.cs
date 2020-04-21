using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Language
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public bool IsDefault { set; get; }
        public List<ProductTranslation> ProductTranslations { set; get; }
        public List<CategoryTranslation> CategoryTranslations { set; get; }

    }
}
