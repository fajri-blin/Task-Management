using Task_Management.Model.Data;

namespace Task_Management.DTOs.CategoryDto;

public class CategoryDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }

    public static explicit operator CategoryDto(Category category)
    {
        return new CategoryDto
        {
            Guid = category.Guid,
            Name = category.Name,
        };
    }

    public static explicit operator Category(CategoryDto categoryDto)
    {
        return new Category
        {
            Guid= categoryDto.Guid,
            Name = categoryDto.Name,
            CreatedAt = DateTime.MinValue,
            ModifiedAt = DateTime.MinValue,
        };
    }
}
