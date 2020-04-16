using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Language
    {
        public int Id { set; get; }
        public Languages Name { set; get; }
        public Languages Default { set; get; }
        public List<ProductTranslation> ProductTranslations { set; get; }
        public List<CategoryTranslation> CategoryTranslations { set; get; }

    }
}
