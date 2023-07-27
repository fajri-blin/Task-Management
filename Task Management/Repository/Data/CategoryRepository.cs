using Task_Management.Contract.Data;
using Task_Management.Data;
using Task_Management.Model.Data;

namespace Task_Management.Repository.Data;

public class CategoryRepository : GeneralRepository<Category>, ICategoryRepository
{
    public CategoryRepository(BookingDbContext bookingDbContext) : base(bookingDbContext)
    {
    }

    public Category? GetByName(string name)
    {
        return _bookingDbContext.Set<Category>().FirstOrDefault(a => a.Name == name);
    }
}
