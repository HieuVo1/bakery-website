using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class CategoryTranslation
    {
        public int CategoryId { set; get; }
        public Category Category { set; get; }
        public Language Language { set; get; }
        public string LanguageId { set; get; }
        public string Name { set; get; }
        public string CategoryUrl { set; get; }
    }
}
