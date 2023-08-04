using System.ComponentModel.DataAnnotations;

namespace ClientSide.ViewModels.Additional
{
    public class CreateAdditionalVM
    {
        public Guid ProgressGuid { get; set; }

        [Required(ErrorMessage = "File is required")]

        [DataType(DataType.Upload)]
        public List<IFormFile> FileData { get; set; }
    }
}
