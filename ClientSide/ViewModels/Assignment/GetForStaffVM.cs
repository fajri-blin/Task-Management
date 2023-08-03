using ClientSide.ViewModels.Progress;

namespace ClientSide.ViewModels.Assignment;

public class GetForStaffVM
{
    public List<ProgressVM> ListProgress { get; set; }
    public string AssignmentName { get; set; }
    public string ManagerName { get; set; }
}
