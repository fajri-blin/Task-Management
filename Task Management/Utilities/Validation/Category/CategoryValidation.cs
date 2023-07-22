using FluentValidation;
using Task_Management.DTOs.CategoryDto;

namespace Task_Management.Utilities.Validation.Category
{
    public class CategoryValidation : AbstractValidator<CategoryDto>
    {
        public CategoryValidation() 
        {
            RuleFor(p => p.Name)
         .NotEmpty();
        }

    }
}
