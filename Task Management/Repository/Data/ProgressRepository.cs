using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class ProgressRepository : GeneralRepository<Progress>, IProgressRepository
{
    public ProgressRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public IEnumerable<Progress> GetByAssignmentForeignKey (Guid guid)
    {
        return _bookingDbContext.Set<Progress>().Where(prog => prog.AssignmentGuid == guid);
    }
}
