using ClientSide.Utilities.Enum;

namespace ClientSide.ViewModels.Account
{
    public class RegisterVM
    {

        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public RoleLevel Role { get; set; }
    }
}
