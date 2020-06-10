using System.ComponentModel.DataAnnotations;

namespace eShopSolution.ViewModel.Contact
{
    public class ContactViewModel
    {
        [Required]
        public string FullName{ get; set; }
        [Required]
        [EmailAddress]
        public string Email{ get; set; }
        [Required]
        [Phone]
        public string Phone{ get; set; }
        [Required]
        public string Messmage{ get; set; }
    }
}
