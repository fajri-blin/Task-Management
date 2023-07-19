using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management.Model;

public abstract class BaseEntity
{
    [Key]
    [Column("guid")]
    public Guid Guid { get; set; }
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    [Column("modified_at")]
    public DateTime ModifiedAt { get; set; }
}
