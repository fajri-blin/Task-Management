using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class AdditionalRepository : GeneralRepository<Additional>, IAdditionalRepository
{
    public AdditionalRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public IEnumerable<Additional> GetByProgressForeignKey(Guid guid)
    {
        return _bookingDbContext.Set<Additional>().Where(a => a.Guid == guid);
    }
}
