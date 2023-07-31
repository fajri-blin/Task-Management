using ClientSide.Utilities.Handlers;
using ClientSide.ViewModels.Dashboard;

namespace ClientSide.Contract
{
    public interface IDashboardRepository
    {
        Task<ResponseHandlers<DashboardMonthMangerVM>> CountMonth(Guid guid);

        Task<ResponseHandlers<DashboardCategoryManagerVM>> CountCategory(Guid guid);
    }
}
