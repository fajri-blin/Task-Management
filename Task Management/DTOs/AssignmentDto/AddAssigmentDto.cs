using Task_Management.DTOs.ProgressDto;
using Task_Management.Model.Data;

namespace Task_Management.DTOs.AssignmentDto;

public class AddAssigmentDto
{
    public Guid ManagerGuid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public ICollection<NewProgressDto> ProgressList { get; set; }

    public static explicit operator Assignment(AddAssigmentDto assignmentDto)
    {
        return new Assignment
        {
            Guid = Guid.NewGuid(),
            ManagerGuid = assignmentDto.ManagerGuid,
            Title = assignmentDto.Title,
            Description = assignmentDto.Description,
            DueDate = assignmentDto.DueDate,
            IsCompleted = assignmentDto.IsCompleted,
        };
    }
}
