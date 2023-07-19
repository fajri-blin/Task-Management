using Task_Management.Model.Data;

namespace Task_Management.DTOs.AccountProgressDto;

public class AccountProgressDto
{
    public Guid Guid { get; set; }
    public Guid? AccountGuid { get; set; }   
    public Guid? ProgressGuid { get; set; }

    public static explicit operator AccountProgressDto(AccountProgress accountProgress)
    {
        return new AccountProgressDto
        {
            Guid = accountProgress.Guid,
            AccountGuid = accountProgress.AccountGuid,
            ProgressGuid = accountProgress.ProgressGuid,
        };
    }

    public static explicit operator AccountProgress(AccountProgressDto accountProgressDto)
    {
        return new AccountProgress
        {
            Guid = accountProgressDto.Guid,
            AccountGuid = accountProgressDto.AccountGuid,
            ProgressGuid = accountProgressDto.ProgressGuid,
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
        };
    }
}
