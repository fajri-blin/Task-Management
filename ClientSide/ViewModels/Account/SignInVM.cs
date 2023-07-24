using System.ComponentModel.DataAnnotations;

namespace ClientSide.ViewModels.Account
{
    public class SignInVM
    {
        [Required]
        public string AccountLogin { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
