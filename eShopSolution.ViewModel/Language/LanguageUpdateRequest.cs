using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Language
{
    public class LanguageUpdateRequest
    {
 
        public string Name { set; get; }
        public bool IsDefault { set; get; }
    }
}
