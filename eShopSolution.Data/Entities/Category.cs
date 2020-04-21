using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
        public int Id { set; get; }
        public bool IsShowOnHome { set; get; }
        public CategoryStatus Status { set; get; }
        public DateTime Created_At { set; get; }
        public string ImagePath { get; set; }
        public List<Product> Products { set; get; }
        public List<CategoryTranslation> CategoryTranslations { set; get; }
    }
}
