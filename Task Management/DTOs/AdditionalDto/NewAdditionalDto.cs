using Task_Management.Model.Data;

namespace Task_Management.DTOs.NewAdditionalDto;

public class NewAdditionalDto
{
    public Guid? ProgressGuid { get; set; }
    public string FileName { get; set; }
    public byte[] FileData { get; set; }
    public string FileType { get; set; }

    public static implicit operator Additional(NewAdditionalDto additionalDto)
    {
        return new Additional
        {
            Guid = Guid.NewGuid(),
            ProgressGuid = additionalDto.ProgressGuid,
            FileName = additionalDto.FileName,
            FileData = additionalDto.FileData,
            FileType = additionalDto.FileType,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };
    }
}
