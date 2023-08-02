using ClientSide.ViewModels.Account;
using FluentValidation;

namespace ClientSide.Utilities.Validation.Account;

public class CheckOTPVMValidation : AbstractValidator<CheckOTPVM>
{
	public CheckOTPVMValidation()
	{
		RuleFor(p => p.OTP)
			.NotEmpty().WithMessage("OTP is required")
			.Must(BeNumeric).WithMessage("OTP must be a numeric value");

		RuleFor(p => p.Email)
			.NotEmpty().WithMessage("Email is required")
			.EmailAddress().WithMessage("Invalid Email Format");
	}

	private bool BeNumeric(int otp)
	{
		return int.TryParse(otp.ToString(), out _);
	}
}
