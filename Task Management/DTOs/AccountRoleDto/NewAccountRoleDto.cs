using Task_Management.Model.Data;

namespace Task_Management.DTOs.AccountRoleDto;

public class NewAccountRoleDto
{
    public Guid AccountGuid { get; set; }   
    public Guid? RoleGuid { get; set; }



    public static implicit operator AccountRole(NewAccountRoleDto accountRoleDto)
    {
        return new AccountRole
        {
            Guid = Guid.NewGuid(),
            AccountGuid = accountRoleDto.AccountGuid,
            RoleGuid = accountRoleDto.RoleGuid ?? null,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };
    }
}
