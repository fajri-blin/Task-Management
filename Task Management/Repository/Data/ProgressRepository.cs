using Microsoft.EntityFrameworkCore;
using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class ProgressRepository : GeneralRepository<Progress>, IProgressRepository
{
    public ProgressRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public IEnumerable<Progress>? GetByAssignmentForeignKey(Guid guid)
    {
        var progresses = _bookingDbContext.Progresses
          .Include(p => p.AccountProgress) // Include the AccountProgress navigation property
          .Where(p => p.AssignmentGuid == guid);

        return progresses.Any() ? progresses : Enumerable.Empty<Progress>();
    }
}
