namespace ClientSide.ViewModels.Progress;

public class AssignStaffVM
{
    public IEnumerable<CurrentStaffVM>? CurrentStaffVMs { get; set; }
    public IEnumerable<AddStaffVM>? StaffListVMs{ get; set;}
}
