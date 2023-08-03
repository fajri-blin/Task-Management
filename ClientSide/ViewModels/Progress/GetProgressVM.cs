namespace ClientSide.ViewModels.Progress;

public class GetProgressVM
{
    public Guid Guid { get; set; }
    public Guid? AssignmentGuid { get; set; }
    public string Description { get; set; }
}
