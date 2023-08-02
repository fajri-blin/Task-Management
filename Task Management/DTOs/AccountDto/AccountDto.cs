using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.DTOs.AccountDto;

public class AccountDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public RoleLevel? Role { get; set; }


    public static explicit operator AccountDto(Account account)
    {
        return new AccountDto
        {
            Guid = account.Guid,
            Name = account.Name,
        };
    }
}
