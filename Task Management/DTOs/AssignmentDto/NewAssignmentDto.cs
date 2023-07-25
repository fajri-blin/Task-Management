using Task_Management.Model.Data;

namespace Task_Management.DTOs.AssignmentDto;

public class NewAssignmentDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Category { get; set; }
    public Guid ManagerGuid { get; set; }

    public static implicit operator Assignment(NewAssignmentDto assignmentDto)
    {
        return new Assignment
        {
            Guid = Guid.NewGuid(),
            Title = assignmentDto.Title,
            Description = assignmentDto.Description,
            DueDate = assignmentDto.DueDate,
            ModifiedAt = DateTime.Now,
            CreatedAt = DateTime.Now,
            ManagerGuid = assignmentDto.ManagerGuid,
        };
    }
}
