using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class AccountProgressRepository : GeneralRepository<AccountProgress>, IAccountProgressRepository
{
    public AccountProgressRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public IEnumerable<AccountProgress> GetByProgressForeignKey (Guid guid)
    {
        return _bookingDbContext.Set<AccountProgress>().Where(acc_prog => acc_prog.Guid == guid);
    }
}
