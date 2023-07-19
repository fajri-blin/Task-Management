using Task_Management.Model;
using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.DTOs.AssignmentDto;

public class NewAssignmentDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public static implicit operator Assignment(NewAssignmentDto assignmentDto)
    {
        return new Assignment
        {
            Guid = Guid.NewGuid(),
            Title = assignmentDto.Title,
            Description = assignmentDto.Description,
            DueDate = assignmentDto.DueDate,
            IsCompleted = assignmentDto.IsCompleted,
            ModifiedAt = DateTime.Now,
            CreatedAt = DateTime.Now,
        };
    }
}
