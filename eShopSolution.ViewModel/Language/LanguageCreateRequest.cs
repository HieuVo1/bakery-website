using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Language
{
    public class LanguageCreateRequest
    {
        [Required]
        public string Id { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        public bool IsDefault { set; get; }
    }
}
