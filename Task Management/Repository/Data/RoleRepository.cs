using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class RoleRepository : GeneralRepository<Role>, IRoleRepository
{
    public RoleRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public Role? GetByName(string name)
    {
        return _bookingDbContext.Set<Role>().FirstOrDefault(role => role.Name.ToLower() == name.ToLower());
    }
}
