namespace ClientSide.ViewModels.Account
{
    public class AccountVM
    {
        public Guid Guid { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public int OTP { get; set; }
        public bool IsUsedOTP { get; set; }
        public string? Password { get; set; }
        public string? ImageProfile { get; set; }
        public Guid RoleGuid { get; set; }
    }
}
