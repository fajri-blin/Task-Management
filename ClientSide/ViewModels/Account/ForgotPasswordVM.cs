using System.ComponentModel.DataAnnotations;

namespace ClientSide.ViewModels.Account;

public class ForgotPasswordVM
{
    [EmailAddress]
    public string Email { get; set; }
}
