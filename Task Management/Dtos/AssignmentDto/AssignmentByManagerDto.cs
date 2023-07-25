namespace Task_Management.Dtos.AssignmentDto
{
    public class AssignmentByManagerDto
    {
        public Guid Guid { get; set; }
        public Guid? ManagerGuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public List<string> category { get; set; }
        public double progress { get; set; }
    }
}
