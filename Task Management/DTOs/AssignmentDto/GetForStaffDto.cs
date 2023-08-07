using Task_Management.DTOs.ProgressDto;
using System.Collections.Generic;

namespace Task_Management.DTOs.AssignmentDto
{
    public class GetForStaffDto
    {
        public List<GetProgressDto> ListProgress { get; set; }
        public Guid AssignmentGuid { get; set; }
        public DateTime? DueDate { get; set; }
        public string AssignmentName { get; set; }
        public string ManagerName { get; set; }
    }
}
