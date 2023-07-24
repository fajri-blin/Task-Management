using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class AssignmentRepository : GeneralRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public IEnumerable<Assignment>? GetByManager(Guid managerId)
    {
        return _bookingDbContext.Set<Assignment>().Where(a => a.ManagerGuid == managerId);
    }
}
