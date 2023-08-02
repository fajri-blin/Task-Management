using Task_Management.Model.Data;
using Task_Management.Utilities.Enum;

namespace Task_Management.Dtos.AccountDto
{
    public class DetailAccountDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public RoleLevel? Role { get; set; }
        public string? ImageProfile { get; set; }

        public static explicit operator DetailAccountDto(Account account)
        {
            return new DetailAccountDto
            {
                Guid = account.Guid,
                Name = account.Name,
                Username = account.Username,
                Email = account.Email,
                ImageProfile = account.ImageProfile,
            };
        }
    }
}
