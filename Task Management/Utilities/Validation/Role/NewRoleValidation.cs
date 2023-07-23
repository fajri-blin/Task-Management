using FluentValidation;
using Task_Management.DTOs.RoleDto;

namespace Task_Management.Utilities.Validation.Role
{
    public class NewRoleValidation : AbstractValidator<NewRoleDto>
    {
        public NewRoleValidation()
        {

            RuleFor(p => p.Name)
          .NotEmpty();
        }
    }
}
