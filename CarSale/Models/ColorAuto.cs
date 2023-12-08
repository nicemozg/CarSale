using System.ComponentModel.DataAnnotations;

namespace CarSale.Models;

public class ColorAuto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string ColorName { get; set; }
}