using Task_Management.Model.Data;

namespace Task_Management.DTOs.CategoryDto;

public class NewCategoryDto
{
    public string Name { get; set; }

    public static implicit operator Category(NewCategoryDto newCategoryDto)
    {
        return new Category
        {
            Guid= Guid.NewGuid(),
            Name = newCategoryDto.Name,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };
    }
}
