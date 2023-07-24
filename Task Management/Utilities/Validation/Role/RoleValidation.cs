using FluentValidation;
using Task_Management.DTOs.RoleDto;

namespace Task_Management.Utilities.Validation.Role
{
    public class RoleValidation : AbstractValidator<RoleDto>
    {
        public RoleValidation()
        {
            RuleFor(p => p.Name)
         .NotEmpty();

        }
    }
}
