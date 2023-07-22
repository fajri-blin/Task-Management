using FluentValidation;
using Task_Management.DTOs.ProgressDto;

namespace Task_Management.Utilities.Validation.Progress
{
    public class ProgressValidation : AbstractValidator<ProgressDto>
    {
        public ProgressValidation()
        {

            RuleFor(p => p.Description)
        .NotEmpty();

            RuleFor(p => p.Status)
        .NotEmpty()
        .IsInEnum();

            RuleFor(p => p.CheckMark)
        .NotEmpty();

        }
    }
}
