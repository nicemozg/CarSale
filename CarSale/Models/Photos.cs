namespace CarSale.Models;

public class Photos
{
    public int Id { get; set; }
    public string? PhotoPath { get; set; } // Путь к фотографии
    public int? ProductId { get; set; }    // Идентификатор продукта
    public Product Product { get; set; }  // Продукт, к которому относится фотография
}