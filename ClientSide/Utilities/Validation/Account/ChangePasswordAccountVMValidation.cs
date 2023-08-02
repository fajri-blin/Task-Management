using ClientSide.ViewModels.Account;
using FluentValidation;

namespace ClientSide.Utilities.Validation.Account;

public class ChangePasswordAccountVMValidations : AbstractValidator<ChangePasswordVM>
{
	public ChangePasswordAccountVMValidations()
	{
		RuleFor(p => p.NewPassword)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
			.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
			.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
			.Matches("[0-9]").WithMessage("Password must contain at least one digit.")
			.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

		RuleFor(p => p.ConfirmPassword)
			.NotEmpty()
			.Equal(model => model.NewPassword).WithMessage("Password and Confirm Password do not match.");
	}
}
