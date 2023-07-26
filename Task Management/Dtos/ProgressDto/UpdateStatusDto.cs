using Task_Management.Utilities.Enum;

namespace Task_Management.Dtos.ProgressDto
{
    public class UpdateStatusDto
    {
        public Guid Guid { get; set; }
        public Guid? AccountGuid { get; set; }
        public StatusEnum Status { get; set; }
        public string? MessageManager { get; set; }
        public string? Additional { get; set; }
    }
}
