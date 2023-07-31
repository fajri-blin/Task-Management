using System.ComponentModel.DataAnnotations;

namespace ClientSide.ViewModels.Assignment;

public class UpdateAssignmentVM
{
    public Guid Guid { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    [Required(ErrorMessage = "The Category field is required.")]
    public List<string> Category { get; set; }
}
