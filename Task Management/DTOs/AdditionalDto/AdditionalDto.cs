using Task_Management.Model.Data;

namespace Task_Management.DTOs.AdditionalDto;

public class AdditionalDto
{
    public Guid Guid { get; set; }
    public Guid? ProgressGuid { get; set; }
    public string FileName { get; set; }
    public byte[] FileData { get; set; }
    public string FileType { get; set; }

    public static explicit operator AdditionalDto(Additional additional)
    {
        return new AdditionalDto
        {
            Guid = additional.Guid,
            ProgressGuid = additional.ProgressGuid,
            FileName = additional.FileName,
            FileData = additional.FileData,
            FileType = additional.FileType,
        };
    }

    public static explicit operator Additional(AdditionalDto additionalDto)
    {
        return new Additional
        {
            Guid = additionalDto.Guid,
            ProgressGuid = additionalDto.ProgressGuid,
            FileName = additionalDto.FileName,
            FileData = additionalDto.FileData,
            FileType = additionalDto.FileType,
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
        };
    }
}
