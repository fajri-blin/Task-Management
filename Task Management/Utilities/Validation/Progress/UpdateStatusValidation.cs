using FluentValidation;
using Task_Management.Dtos.ProgressDto;

namespace Task_Management.Utilities.Validation.Progress
{
    public class UpdateStatusValidation : AbstractValidator<UpdateStatusDto>
    {
        public UpdateStatusValidation()
        {

            RuleFor(p => p.Guid)
        .NotEmpty();
        }
    }
}
