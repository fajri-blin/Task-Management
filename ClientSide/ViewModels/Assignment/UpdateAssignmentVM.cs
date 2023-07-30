namespace ClientSide.ViewModels.Assignment;

public class UpdateAssignmentVM
{
    public Guid Guid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Categories { get; set; }
}
