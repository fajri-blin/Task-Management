using Task_Management.Contract.Data;
using Task_Management.Dtos.Dashboard;

namespace Task_Management.Service
{
    public class DashboardService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAssignMapRepository _assignMapRepository;
        private readonly ICategoryRepository _categoryRepository;

        public DashboardService(IAssignmentRepository assignmentRepository, IAssignMapRepository assignMapRepository, ICategoryRepository categoryRepository)
        {
            _assignmentRepository = assignmentRepository;
            _assignMapRepository = assignMapRepository;
            _categoryRepository = categoryRepository;
        }

        public AssignmentRateDto? CountMonthManager(Guid guid)
        {
            var assignments = _assignmentRepository.GetByManager(guid);

            DateTime startDate = new DateTime(DateTime.Now.Year, 1, 1);

            DateTime endDate = startDate.AddYears(1);

            assignments = assignments.Where(a => a.CreatedAt >= startDate && a.CreatedAt < endDate);

            var months = new List<string>
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

            var monthlyTotals = months.GroupJoin(
                assignments,
                month => month,
                assignment => assignment.CreatedAt.ToString("MMM"),
                (month, assignmentMonth) => assignmentMonth.Count()
                ).ToList();


            var dto = new AssignmentRateDto
            {
                Month = months,
                Count = monthlyTotals
            };

            return dto;
        }

        public CountTop3CategoryDto? CountCategory(Guid guid)
        {
            var assignments = _assignmentRepository.GetByManager(guid);
            var assignMaps = _assignMapRepository.GetAll();
            var categories = _categoryRepository.GetAll();

            var entity = (
                from assignment in assignments
                join assignMap in assignMaps on assignment.Guid equals assignMap.AssignmentGuid
                join category in categories on assignMap.CategoryGuid equals category.Guid
                group category by category.Name into g
                select new
                {
                    category = g.Key,
                    count = g.Count()
                }).OrderByDescending(c => c.count).ToList();

            var totalOtherCount = entity.Skip(3).Sum(c => c.count);
            var top3Categories = entity.Take(3).ToList();

            var dto = new CountTop3CategoryDto
            {
                CategoryName = top3Categories.Select(c => c.category).ToList(),
                Count = top3Categories.Select(c => c.count).ToList(),
            };
            if (totalOtherCount > 0)
            {
                dto.CategoryName.Add("Other");
                dto.Count.Add(totalOtherCount);
            }

            return dto;
        }
    }
}
