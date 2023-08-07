using ClientSide.ViewModels.Progress;

namespace ClientSide.ViewModels.Assignment;

public class GetForStaffVM
{
    public List<GetProgressVM> ListProgress { get; set; }
    public Guid AssignmentGuid { get; set; }
    public DateTime? DueDate { get; set; }
    public string AssignmentName { get; set; }
    public string ManagerName { get; set; }
}
