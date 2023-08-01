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
            _accountRepository = accountRepository;
            RuleFor(p => p.Username)
          .Must(BeUniqueProperty).WithMessage("'Username' already registered");

            RuleFor(p => p.Email)
          .Must(BeUniqueProperty).WithMessage("'Email' already registered")
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

        private bool BeUniqueProperty(string property)
        {
            return !_accountRepository.IsDuplicateValue(property);
        }

        private bool ValidateImage(IFormFile file)
        {
            if (file is null)
            {
                return true;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(fileExtension);
        }
    }
}
