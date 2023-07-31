namespace Task_Management.Dtos.AccountDto
{
    public class UpdateAccountDto
    {
        public Guid Guid { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public IFormFile? ImageProfile { get; set; }
    }
}
