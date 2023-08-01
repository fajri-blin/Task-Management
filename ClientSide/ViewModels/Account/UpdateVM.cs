using ClientSide.Utilities.Enum;

namespace ClientSide.ViewModels.Account
{
    public class UpdateVM
    {
        public Guid Guid { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public RoleLevel Role { get; set; }
        public string? ImageProfile { get; set; }
    }
}
