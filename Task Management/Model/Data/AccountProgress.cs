using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management.Model.Data;

[Table("tb_tr_account_progress")]
public class AccountProgress : BaseEntity
{
    [Column("account_guid")]
    public Guid? AccountGuid { get; set; }

    [Column("progress_guid")]
    public Guid? ProgressGuid { get; set; }

    //Cardinality
    public Account? Account { get; set; }
    public Progress? Progress { get; set; }

}
