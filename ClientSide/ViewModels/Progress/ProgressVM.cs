using Task_Management.Utilities.Enum;
using Newtonsoft.Json;

namespace ClientSide.ViewModels.Progress;

public class ProgressVM
{
    public Guid Guid { get; set; }
    public Guid? AssignmentGuid { get; set; }
    public string Description { get; set; }
    public StatusEnum Status { get; set; }
    public string? Additional { get; set; }
    public string? MessageManager { get; set; }
    public List<Guid> StaffGuids { get; set; }

    /*  public DateTime DueDate { get; set; }
      public double Progress { get; set; }*/
}



