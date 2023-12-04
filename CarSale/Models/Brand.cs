using System.ComponentModel.DataAnnotations;

namespace CarSale.Models;

public class Brand
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не может быть пустым")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина должна быть от 3 до 20 символов")]
    public string BrandName { get; set; }
}