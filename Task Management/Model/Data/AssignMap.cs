using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management.Model.Data;

[Table("tb_tr_assign_maps")]
public class AssignMap : BaseEntity
{
    [Column("assignment_guid")]
    public Guid? AssignmentGuid { get; set; }
    [Column("category_guid")]
    public Guid? CategoryGuid { get; set; }

    //Cardinality
    public Assignment? Assignment { get; set; }
    public Category? Category { get; set; }
}
