using Task_Management.Model.Data;

namespace Task_Management.DTOs.AssignMapDto;

public class NewAssignMapDto
{
    public Guid AssignmentGuid { get; set; }
    public Guid CategoryGuid { get; set; }

    public static implicit operator AssignMap(NewAssignMapDto assignMapDto)
    {
        return new AssignMap
        {
            Guid = Guid.NewGuid(),
            AssignmentGuid = assignMapDto.AssignmentGuid,
            CategoryGuid = assignMapDto.CategoryGuid,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };
    }
}
