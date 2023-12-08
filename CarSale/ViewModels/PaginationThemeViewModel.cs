using CarSale.Models;

namespace CarSale.ViewModels;

public class PaginationThemeViewModel
{
    public List<Product> Products { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}