using Task_Management.Model.Data;

namespace Task_Management.DTOs.RoleDto;

public class RoleDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }

    public static explicit operator RoleDto(Role role)
    {
        return new RoleDto
        {
            Guid = role.Guid,
            Name = role.Name,
        };
    }

    public static explicit operator Role(RoleDto roleDto)
    {
        return new Role
        {
            Guid= roleDto.Guid,
            Name = roleDto.Name,
            ModifiedAt = DateTime.MinValue,
            CreatedAt = DateTime.MinValue,
        };
    }
}
