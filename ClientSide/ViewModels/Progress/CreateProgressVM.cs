using System.ComponentModel.DataAnnotations;
using Task_Management.Utilities.Enum;

namespace ClientSide.ViewModels.Progress
{
    public class CreateProgressVM
    {
        [Required(ErrorMessage = "AssignmentGuid is required")]
        public Guid? AssignmentGuid { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }
        public string? Additional { get; set; }
        public string? MessageManager { get; set; }
    }
}
