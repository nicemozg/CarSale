using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarSale.Models;

public class Product
{
    public int Id { get; set; }
    public Guid ProductId { get; set; }
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string Name { get; set; }
    public string? Description { get; set; }

    [Display(Name = "Изображение")]
    [NotMapped]
    public IFormFile? AvatarFile { get; set; }
    
    [Display(Name = "Изображение")]
    public string? AvatarFileName { get; set; }
    public DateTime Creation { get; set; }
    public DateTime? Update { get; set; }
   
    public decimal Price { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Брэнд не может быть пустым")]
    public int BrandId { get; set; }
    
    public Brand Brand { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Категория не может быть пустым")]
    public int CategoryId { get; set; }
    
    public Category Category { get; set; }

    
    public Product()
    {
    }
}