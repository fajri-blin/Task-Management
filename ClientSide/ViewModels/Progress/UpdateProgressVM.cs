using Task_Management.Utilities.Enum;

namespace ClientSide.ViewModels.Progress
{
    public class UpdateProgressVM
    {
        public Guid Guid { get; set; }
        public Guid? AssignmentGuid { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }
        public string? Additional { get; set; }
        public string? MessageManager { get; set; }
   
    }
}
