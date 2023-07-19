using Task_Management.Model.Data;

namespace Task_Management.DTOs.AssignMapDto;

public class AssignMapDto
{
    public Guid Guid { get; set; }
    public Guid? AssignmentGuid { get; set; }
    public Guid? CategoryGuid { get; set; }

    public static explicit operator AssignMapDto(AssignMap assignMap)
    {
        return new AssignMapDto
        {
            Guid = assignMap.Guid,
            AssignmentGuid = assignMap.AssignmentGuid,
            CategoryGuid = assignMap.CategoryGuid,
        };
    }

    public static explicit operator AssignMap(AssignMapDto assignMapDto)
    {
        return new AssignMap
        {
            Guid = assignMapDto.Guid,
            AssignmentGuid = assignMapDto.AssignmentGuid,
            CategoryGuid = assignMapDto.CategoryGuid,
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
        };
    }
}
