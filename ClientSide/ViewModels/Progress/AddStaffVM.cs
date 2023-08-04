namespace ClientSide.ViewModels.Progress
{
    public class AddStaffVM
    {
        public Guid Guid { get; set; }
        public Guid? AccountGuid { get; set; }
        public Guid? ProgressGuid { get; set; }
        public string Name { get; set; }
    }
}
