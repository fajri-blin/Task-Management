using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management.Model.Data;

[Table("tb_m_roles")]
public class Role : BaseEntity
{
    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    //Cardinality
    public ICollection<AccountRole>? AccountRoles { get; set; }
}
