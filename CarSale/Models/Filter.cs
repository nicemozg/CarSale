namespace CarSale.Models;

public class Filter
{
    public int Id { get; set; }
    public string CookieId { get; set; }
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public int? MotorId { get; set; }

    public Filter()
    {
    }
}