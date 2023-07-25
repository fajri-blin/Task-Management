using FluentValidation;
using Task_Management.Dtos.AccountDto;

namespace Task_Management.Utilities.Validation.Account
{
    public class CheckOtpValidation : AbstractValidator<CheckOtp>
    {
        public CheckOtpValidation()
        {

            RuleFor(p => p.OTP)
          .NotEmpty().WithMessage("OTP is required.");

            RuleFor(p => p.Email)
          .NotEmpty().WithMessage("Email is required.");
        }
    }
}
