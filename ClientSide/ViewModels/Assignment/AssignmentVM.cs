namespace ClientSide.ViewModels.Assignment;

public class AssignmentVM
{
    public Guid Guid { get; set; }
    public string? Name { get; internal set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public List<string> Category { get; set; }
    public double Progress { get; set; }
}
