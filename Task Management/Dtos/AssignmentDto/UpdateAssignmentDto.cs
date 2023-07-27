using Task_Management.Model.Data;

namespace Task_Management.DTOs.AssignmentDto;

public class UpdateAssignmentDto
{
    public Guid Guid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Category { get; set; }

    public static implicit operator Assignment(UpdateAssignmentDto updateAssignmentDto)
    {
        return new Assignment
        {
            Guid = updateAssignmentDto.Guid,
            Title = updateAssignmentDto.Title,
            Description = updateAssignmentDto.Description,
            DueDate = updateAssignmentDto.DueDate,
            ModifiedAt = DateTime.Now,
            CreatedAt = DateTime.Now,
        };
    }
}
