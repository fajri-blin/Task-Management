using Microsoft.EntityFrameworkCore;
using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
{
    public AccountRoleRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public IEnumerable<AccountRole> GetAccountRolesByAccountGuid(Guid guid)
    {
        return _bookingDbContext.Set<AccountRole>().Where(ar => ar.AccountGuid == guid);
    }
}
