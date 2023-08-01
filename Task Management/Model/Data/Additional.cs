using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management.Model.Data;

[Table("tb_m_additional")]
public class Additional : BaseEntity
{
    [Column("progress_guid")]
    public Guid? ProgressGuid { get; set; }

    [Column("filename", TypeName = "nvarchar(255)")]
    public string FileName { get; set; }

    [Column("filedata", TypeName = "nvarchar(255)")]
    public string FileData { get; set; }


    //Cardinality
    public Progress? Progress { get; set; }
}
