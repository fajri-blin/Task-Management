using System.ComponentModel.DataAnnotations.Schema;
using Task_Management.Utilities.Enum;

namespace Task_Management.Model.Data;

[Table("tb_m_assignemts")]
public class Assignment : BaseEntity
{
    [Column("manager_guid")]
    public Guid? ManagerGuid { get; set; }

    [Column("title", TypeName = "nvarchar(50)")]
    public string Title { get; set; }

    [Column("description", TypeName = "nvarchar(255)")]
    public string Description { get; set; }

    [Column("due_date")]
    public DateTime DueDate { get; set; }

    [Column("is_completed")]
    public bool IsCompleted { get; set; }

    //Cardinality
    public ICollection<Progress>? Progresses { get; set; }
    public ICollection<AssignMap>? AssignMaps { get; set; }
    public Account? Account { get; set; }
}
