using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories;

public class CreateCategoryRequest : Request
{
    [Required(ErrorMessage = "Título inválido!")]
    [MaxLength(100, ErrorMessage = "O título da categoria deve conter até 100 caracteres!")]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000, ErrorMessage = "A descrição da categoria deve conter até 1000 caracteres!")]
    public string? Description { get; set; }
}
