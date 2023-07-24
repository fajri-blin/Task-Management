using System.ComponentModel.DataAnnotations;
using Task_Management.Utilities.Enum;

namespace Task_Management.DTOs.AccountDto;

public class RegisterDto
{
    public string Username { get; set; }    
    [EmailAddress]
    public string Email { get; set; }
    public string Name { get; set; }
    //[PasswordPolicy]
    public string Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string Role { get; set; }
    public string? ImageProfile { get;set; }
}
