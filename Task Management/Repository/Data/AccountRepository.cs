using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    public AccountRepository(BookingDbContext context) : base(context) { }

    public Account? GetEmailorUsername(string account)
    {
        return _bookingDbContext.Set<Account>().FirstOrDefault(ac => ac.Username == account || ac.Email == account);
    }

}
