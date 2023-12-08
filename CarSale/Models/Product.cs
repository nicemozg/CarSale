using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarSale.Controllers;

namespace CarSale.Models;

public class Product
{
    public int Id { get; set; }
    public int Years { get; set; }
    public string Transmission { get; set; }
    public string SteeringWheel { get; set; }
    public string? Description { get; set; }
    public string? Volume { get; set; }

    [Display(Name = "Изображение")]
    [NotMapped]
    public IFormFile? AvatarFile { get; set; }
    
    [Display(Name = "Изображение")]
    public string? AvatarFileName { get; set; }
    //------------------------------------------
    [Display(Name = "Изображение")]
    [NotMapped]
    public IFormFile? InfoFile { get; set; }
    
    [Display(Name = "Изображение")]
    public string? InfoFileName { get; set; }
    //------------------------------------------
    
    public DateTime Creation { get; set; }
    public DateTime? Update { get; set; }
   
    public int? Price { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Брэнд не может быть пустым")]
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Категория не может быть пустым")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Тип кузова не может быть пустым")]
    public int TypeAutoId { get; set; }
    public TypeAuto TypeAuto { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Тип кузова не может быть пустым")]
    public int ModelAutoId { get; set; }
    public ModelAuto ModelAuto { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Тип двигателя не может быть пустым")]
    public int TypeMotorId { get; set; }
    public TypeMotor TypeMotor { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Тип двигателя не может быть пустым")]
    public int ColorAutoId { get; set; }
    public ColorAuto ColorAuto { get; set; }
    
    [NotMapped] // Используйте атрибут NotMapped, чтобы EF Core игнорировал это свойство при создании схемы базы данных
    public List<IFormFile> PhotoFiles { get; set; }
    [NotMapped]
    public List<Photos> Photos { get; set; } = new List<Photos>();
    
    public Product()
    {
    }
}