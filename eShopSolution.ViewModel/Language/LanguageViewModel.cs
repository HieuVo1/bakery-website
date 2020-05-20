using eShopSolution.Data.Enums;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Language
{
    public class LanguageViewModel
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public bool IsDefault { set; get; }
    }
}
