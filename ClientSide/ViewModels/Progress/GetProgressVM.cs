using Task_Management.Utilities.Enum;

namespace ClientSide.ViewModels.Progress;

public class GetProgressVM
{
    public Guid Guid { get; set; }
    public string Description { get; set; }

    public StatusEnum StatusProgress { get; set; }
}
