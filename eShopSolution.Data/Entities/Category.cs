using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string CategoryUrl { set; get; }
        public bool IsShowOnHome { set; get; }
        public CategoryStatus Status { set; get; }
        public DateTime Created_At { set; get; }
    }
}
