using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModel.Language
{
    public class LanguageUpdateRequest
    {

        [Required]
        public string Name { set; get; }
        [Required]
        public bool IsDefault { set; get; }
    }
}
