/*using FluentValidation;
using Task_Management.Contract.Data;
using Task_Management.DTOs.NewAdditionalDto;

namespace Task_Management.Utilities.Validation.Account
{
    public class NewAdditionalValidation : AbstractValidator<NewAdditionalDto>
    {
        public NewAdditionalValidation(IAccountRepository accountRepository)
        {
            RuleFor(p => p.ProgressGuid)
           .NotEmpty().WithMessage("Guid is required.");

            RuleFor(p => p.FileName)
            .NotNull().WithMessage("File is required")
            .Must(ValidateImage).WithMessage("Invalid photo extension. Only JPG and JPEG files are allowed.");

        }

        private bool ValidateImage(List<IFormFile> file)
        {
            if (file is null)
            {
                return true;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf", "xls", "xlsx" };

            var count = 0;

            foreach (var fileData in file)
            {
                var fileExtension = System.IO.Path.GetExtension(fileData.FileName).ToLowerInvariant();
                if (allowedExtensions.Contains(fileExtension))
                {
                    count++;
                }
            }

            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
*/