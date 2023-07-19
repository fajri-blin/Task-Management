using Task_Management.Model;
using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.DTOs.AssignmentDto;

public class AssignmentDto
{
    public Guid Guid { get; set; }
    public Guid? ManagerGuid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public static explicit operator AssignmentDto(Assignment assignment)
    {
        return new AssignmentDto
        {
            Guid = assignment.Guid,
            Title = assignment.Title,
            Description = assignment.Description,
            DueDate = assignment.DueDate,
            IsCompleted = assignment.IsCompleted,
        };
    }

    public static explicit operator Assignment(AssignmentDto taskDto)
    {
        return new Assignment
        {
            Guid = taskDto.Guid,
            Title = taskDto.Title,
            Description = taskDto.Description,
            DueDate = taskDto.DueDate,
            IsCompleted = taskDto.IsCompleted,
            ModifiedAt = DateTime.MinValue,
            CreatedAt = DateTime.MinValue,
        };
    }
}
