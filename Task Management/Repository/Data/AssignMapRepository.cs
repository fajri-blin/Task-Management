using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class AssignMapRepository : GeneralRepository<AssignMap>, IAssignMapRepository
{
    public AssignMapRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public IEnumerable<AssignMap> GetByAssignmentForeignKey(Guid guid)
    {
        return _bookingDbContext.Set<AssignMap>().Where(a => a.AssignmentGuid == guid);
    }
}
