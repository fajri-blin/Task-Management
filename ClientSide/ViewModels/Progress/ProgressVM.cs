using Task_Management.Utilities.Enum;

namespace ClientSide.ViewModels.Progress;

public class ProgressVM
{
    public Guid Guid { get; set; }
    public Guid? AssignmentGuid { get; set; }
    public string Description { get; set; }
    public StatusEnum Status { get; set; }
    public Guid? Additional { get; set; }


}
