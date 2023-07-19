using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management.Model.Data;

[Table("tb_m_categories")]
public class Category : BaseEntity
{
    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    //Cardinality
    public ICollection<AssignMap>? AssignMaps { get; set; }

}
