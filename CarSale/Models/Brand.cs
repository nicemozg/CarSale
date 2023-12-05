using System.ComponentModel.DataAnnotations;

namespace CarSale.Models;

public class Brand
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string BrandName { get; set; }
}