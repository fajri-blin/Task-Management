using FluentValidation;
using Task_Management.DTOs.ProgressDto;

namespace Task_Management.Utilities.Validation.Progress
{
    public class NewProgressValidation : AbstractValidator<NewProgressDto>
    {
        public NewProgressValidation()
        {

            RuleFor(p => p.Description)
        .NotEmpty();

            RuleFor(p => p.CheckMark)
        .NotEmpty();
        }
    }
}
