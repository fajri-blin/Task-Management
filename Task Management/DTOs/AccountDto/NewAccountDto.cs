using Task_Management.Model.Data;
using Task_Management.Utilities;

namespace Task_Management.DTOs.AccountDto;

public class NewAccountDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public int OTP { get; set; }
    public bool IsUsedOTP { get; set; }
    public string? Password { get; set; }
    public string? ImageProfile { get; set; }


    public static implicit operator Account(NewAccountDto account)
    {
        return new Account
        {
            Guid = Guid.NewGuid(),
            Username = account.Username,
            Email = account.Email,
            OTP = account.OTP,
            IsUsedOTP = account.IsUsedOTP,
            Password = Hashing.HashPassword(account.Password),
            ImageProfile = account.ImageProfile ?? null,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };
    }
}
