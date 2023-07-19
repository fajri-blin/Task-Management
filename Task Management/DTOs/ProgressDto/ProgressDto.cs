using Task_Management.Model;
using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.DTOs.ProgressDto;

public class ProgressDto
{
    public Guid Guid { get; set; }
    public Guid? AssignmentGuid { get; set; }
    public string Description { get; set; }
    public StatusEnum Status { get; set; }
    public string? Additional { get; set; }
    public bool CheckMark { get; set; }
    public string? MessageManager { get; set; }

    public static explicit operator ProgressDto(Progress progress)
    {
        return new ProgressDto
        {
            Guid = progress.Guid,
            AssignmentGuid = progress.AssignmentGuid,
            Description = progress.Description,
            Status = progress.Status,
            Additional = progress.Additional,
            CheckMark = progress.CheckMark,
            MessageManager = progress.MessageManager,

        };
    }

    public static explicit operator Progress(ProgressDto progressDto)
    {
        return new Progress
        {
            Guid = progressDto.Guid,
            AssignmentGuid = progressDto.AssignmentGuid,
            Description = progressDto.Description,
            Status = progressDto.Status,
            Additional = progressDto.Additional,
            CheckMark = progressDto.CheckMark,
            MessageManager = progressDto.MessageManager,
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
        };
    }
}
