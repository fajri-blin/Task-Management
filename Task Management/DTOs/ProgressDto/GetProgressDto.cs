using Task_Management.Utilities.Enum;

namespace Task_Management.DTOs.ProgressDto
{
    public class GetProgressDto
    {
        public Guid Guid { get; set; }
        public string Description { get; set; }
        public StatusEnum StatusProgress { get; set; }
    }
}
