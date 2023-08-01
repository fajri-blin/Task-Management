using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management.Model.Data;

[Table("tb_m_accounts")]
public class Account : BaseEntity
{
    [Column("username", TypeName = "nvarchar(255)")]
    public string Username { get; set; }

    [Column("email", TypeName = "nvarchar(255)")]
    public string Email { get; set; }

    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    [Column("otp")]
    public int OTP { get; set; }

    [Column("is_used_otp")]
    public bool IsUsedOTP { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [Column("image_profile")]
    public string? ImageProfile { get; set; }

    [Column("role_guid")]
    public Guid? RoleGuid { get; set; }

    //Cardinality
    public Role? Role { get; set; }
    public ICollection<AccountProgress>? AccountProgresses { get; set; }
    public ICollection<Assignment>? Assignments { get; set; }

}
