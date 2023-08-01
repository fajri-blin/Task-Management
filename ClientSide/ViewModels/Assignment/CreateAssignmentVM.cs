namespace ClientSide.ViewModels.Assignment;

public class CreateAssignmentVM
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }

    private List<string> _category;
    public List<string> Category
    {
        get => _category;
        set => _category = value?.Select(c => c.ToUpper()).ToList();
    }
    public Guid ManagerGuid { get; set; }
}
