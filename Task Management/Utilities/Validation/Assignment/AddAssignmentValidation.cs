using FluentValidation;
using Task_Management.DTOs.AssignmentDto;

namespace Task_Management.Utilities.Validation.Assignment
{
    public class AddAssignmentValidation : AbstractValidator<AddAssigmentDto>
    {
        public AddAssignmentValidation()
        {

            RuleFor(p => p.Title)
          .NotEmpty();

            RuleFor(p => p.Description)
          .NotEmpty();

            RuleFor(p => p.DueDate)
          .NotEmpty();

        }
    }
}
