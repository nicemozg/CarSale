using System.ComponentModel.DataAnnotations;

namespace CarSale.Models;

public class ModelAuto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string ModelName { get; set; }
}