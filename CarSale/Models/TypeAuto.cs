using System.ComponentModel.DataAnnotations;

namespace CarSale.Models;

public class TypeAuto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не может быть пустым")]
    public string TypeName { get; set; }
}