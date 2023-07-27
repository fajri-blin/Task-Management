namespace ClientSide.ViewModels.Assignment;

public class CreateAssignmentVM
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Category { get; set; }
    public Guid ManagerGuid { get; set; }
}
