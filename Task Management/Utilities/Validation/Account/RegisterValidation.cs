using FluentValidation;
using Task_Management.Contract.Data;
using Task_Management.DTOs.AccountDto;

namespace Task_Management.Utilities.Validation.Account
{
    public class RegisterValidation : AbstractValidator<RegisterDto>
    {
        private readonly IAccountRepository _accountRepository;

        public RegisterValidation(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

            RuleFor(p => p.Username)
          .NotEmpty()
          .Must(BeUniqueProperty).WithMessage("'Username' already registered");

            RuleFor(p => p.Email)
          .NotEmpty()
          .Must(BeUniqueProperty).WithMessage("'Email' already registered")
          .EmailAddress();

            RuleFor(p => p.Name)
          .NotEmpty();

            RuleFor(p => p.Password)
          .NotEmpty().WithMessage("Password is required.")
          .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
          .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
          .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
          .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
          .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(p => p.ConfirmPassword)
          .NotEmpty()
          .Equal(model => model.Password).WithMessage("Password and Confirm Password do not match.");

            RuleFor(p => p.Role)
          .NotEmpty();
        }

        private bool BeUniqueProperty(string property)
        {
            return _accountRepository.IsDuplicateValue(property);
        }

    }
}
