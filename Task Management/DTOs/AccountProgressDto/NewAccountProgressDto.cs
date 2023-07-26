using Task_Management.Model.Data;

namespace Task_Management.DTOs.AccountProgressDto;

public class NewAccountProgressDto
{
    public Guid AccountGuid { get; set; }
    public Guid? ProgressGuid { get; set; }



    public static implicit operator AccountProgress(NewAccountProgressDto accountProgressDto)
    {
        return new AccountProgress
        {
            Guid = Guid.NewGuid(),
            AccountGuid = accountProgressDto.AccountGuid,
            ProgressGuid = accountProgressDto.ProgressGuid ?? null,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };
    }
}
