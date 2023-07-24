using FluentValidation;
using Task_Management.DTOs.CategoryDto;

namespace Task_Management.Utilities.Validation.Category
{
    public class NewCategoryValidation : AbstractValidator<NewCategoryDto>
    {
        public NewCategoryValidation() 
        {
            RuleFor(p => p.Name)
        .NotEmpty();
        }
    }
}
