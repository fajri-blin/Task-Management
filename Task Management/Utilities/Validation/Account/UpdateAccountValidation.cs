using FluentValidation;
using Task_Management.Contract.Data;
using Task_Management.Dtos.AccountDto;

namespace Task_Management.Utilities.Validation.Account
{
    public class UpdateAccountValidation : AbstractValidator<UpdateAccountDto>
    {
        private readonly IAccountRepository _accountRepository;
        public UpdateAccountValidation(IAccountRepository accountRepository)
        {
            RuleFor(p => p.Guid)
           .NotEmpty().WithMessage("Guid is required.")
           .Must(GuidConvert).WithMessage("Guid is required");

            _accountRepository = accountRepository;
            RuleFor(p => p.Username)
          .Must((model, username) => BeUniqueProperty(username, Guid.Parse(model.Guid))).WithMessage("'Username' already registered");

            RuleFor(p => p.Email)
          .Must((model, email) => BeUniqueProperty(email, Guid.Parse(model.Guid))).WithMessage("'Email' already registered")
          .EmailAddress();

            RuleFor(p => p.Password)
          .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
          .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
          .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
          .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
          .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(p => p.ConfirmPassword)
          .NotEmpty().WithMessage("Confirm Password is required.")
          .When(model => !string.IsNullOrEmpty(model.Password))
          .Equal(model => model.Password).WithMessage("Password and Confirm Password do not match.");

            RuleFor(p => p.ImageProfile)
        .Must(ValidateImage).WithMessage("Invalid photo extension. Only JPG and JPEG files are allowed.");

        }

        private bool BeUniqueProperty(string property, Guid guid)
        {
            var account = _accountRepository.GetByGuid(guid);

            // Check if the existing account has the same username or email
            if (property == account.Username || property == account.Email)
            {
                return true; // The property is the same as the existing account's username or email
            }

            return !_accountRepository.IsDuplicateValue(property);
        }

        private bool ValidateImage(IFormFile file)
        {
            if (file is null)
            {
                return true;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(fileExtension);
        }

        private bool GuidConvert(string guid)
        {
            return Guid.TryParse(guid, out _);
        }
    }
}
