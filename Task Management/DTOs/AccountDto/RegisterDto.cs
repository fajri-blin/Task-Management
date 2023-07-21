using System.ComponentModel.DataAnnotations;
using Task_Management.Utilities.Enum;

namespace Task_Management.DTOs.AccountDto;

public class RegisterDto
{
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    //[PasswordPolicy]
    public string Password { get; set; }
    [Required]
    public string? ConfirmPassword { get; set; }
    public string Role { get; set; }
    public string? ImageProfile { get;set; }
}
