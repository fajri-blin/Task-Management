using ClientSide.Utilities.Enum;

namespace ClientSide.ViewModels.Account
{
    public class AccountVM
    {
        public Guid Guid { get; set; }
        public string? Name { get; set; }
        public RoleLevel Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}
