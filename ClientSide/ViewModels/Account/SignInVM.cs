using System.ComponentModel.DataAnnotations;

namespace ClientSide.ViewModels.Account
{
    public class SignInVM
    {
        [Required(ErrorMessage = "Email or Username is required.")]
        [Display(Name = "Email or Username")]
        public string AccountLogin { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
