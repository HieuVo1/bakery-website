using System.ComponentModel.DataAnnotations;

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
