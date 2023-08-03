using System.ComponentModel.DataAnnotations;

namespace ClientSide.ViewModels.Account;

public class ChangePasswordVM
{
    public string Email { get; set; }
    public int OTP { get; set; }
    [Required(ErrorMessage = "New password is required.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}
