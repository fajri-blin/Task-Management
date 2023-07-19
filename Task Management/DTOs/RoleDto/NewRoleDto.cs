using Task_Management.Model.Data;

namespace Task_Management.DTOs.RoleDto;

public class NewRoleDto
{
    public string Name { get; set; }

    public static implicit operator Role(NewRoleDto roleDto)
    {
        return new Role
        {
            Guid= Guid.NewGuid(),
            Name = roleDto.Name,
            ModifiedAt = DateTime.MinValue,
            CreatedAt = DateTime.MinValue,
        };
    }
}
