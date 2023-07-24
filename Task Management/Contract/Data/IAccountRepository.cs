using Task_Management.Model.Data;

namespace Task_Management.Contract.Data;

public interface IAccountRepository : IGeneralRepository<Account>
{
    Account? GetEmailorUsername(string account);
    bool IsDuplicateValue(string account);
}
