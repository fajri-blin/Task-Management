using System.ComponentModel.DataAnnotations.Schema;
using Task_Management.Utilities.Enum;

namespace Task_Management.Model.Data;

[Table("tb_m_progresses")]
public class Progress : BaseEntity
{
    [Column("assignment_guid")]
    public Guid? AssignmentGuid { get; set; }

    [Column("description", TypeName = "nvarchar(150)")]
    public string Description { get; set; }

    [Column("status")]
    public StatusEnum Status { get; set; }

    [Column("additional", TypeName = "nvarchar(255)")]
    public string? Additional { get; set; }

    [Column("check_mark")]
    public bool CheckMark { get; set; }

    [Column("manager_message", TypeName = "nvarchar(255)")]
    public string? MessageManager { get; set; }

    //Cardinality
    public ICollection<AccountProgress>? AccountProgress { get; set; }
    public Assignment? Assignment { get; set; }
    public ICollection<Additional> Additionals { get; set; }

}
