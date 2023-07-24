namespace ClientSide.ViewModels.Assignment;

public class AssignmentVM
{
    public Guid Guid { get; set; }
    public Guid? ManagerGuid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}
