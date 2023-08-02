using ClientSide.ViewModels.Account;
using FluentValidation;

namespace ClientSide.Utilities.Validation.Account;

public class ForgotPasswordVMValidation : AbstractValidator<ForgotPasswordVM>
{
	public ForgotPasswordVMValidation()
	{
		RuleFor(p => p.Email)
			.NotEmpty().WithMessage("Email is not empty")
			.EmailAddress().WithMessage("Invalid Email Format");
	}
}
