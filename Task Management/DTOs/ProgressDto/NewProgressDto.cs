using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.DTOs.ProgressDto;

public class NewProgressDto
{
    public Guid AssignmentGuid { get; set; }
    public string Description { get; set; }
    public StatusEnum Status { get; set; }
    public string? Additional { get; set; }
    public bool CheckMark { get; set; }
    public string? MessageManager { get; set; }


    public static implicit operator Progress(NewProgressDto NewProgressDto)
    {
        return new Progress
        {
            Guid = Guid.NewGuid(),
            AssignmentGuid = NewProgressDto.AssignmentGuid,
            Description = NewProgressDto.Description,
            Status = NewProgressDto.Status,
            Additional = NewProgressDto.Additional,
            CheckMark = NewProgressDto.CheckMark,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };
    }
}
