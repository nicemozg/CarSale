using CarSale.Context;
using CarSale.Enums;
using CarSale.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarSale.Controllers;

public class ProductsController : Controller
{
    private Car_Sale_Context _db;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ProductsController(Car_Sale_Context db, UserManager<User> userManager, IWebHostEnvironment hostingEnvironment)
    {
        _db = db;
        _userManager = userManager;
        _hostingEnvironment = hostingEnvironment;
    }

   [HttpGet]
    public IActionResult Index()
    {
        List<Product> products = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .ToList();

        List<Brand> brands = _db.Brands.ToList();
        ViewBag.Brands = brands;
        List<Category> categories = _db.Categories.ToList();
        ViewBag.Categories = categories;

        return View(products);
    }
    
    
    //----Страничка Index--[HttpPost]-----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Index(int? category, int? brand)
    {
        List<Product> products = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .ToList();

        if (category.HasValue)
        {
            products = products.Where(p => p.Category.Id == category.Value).ToList();
        }

        if (brand.HasValue)
        {
            products = products.Where(p => p.Brand.Id == brand.Value).ToList();
        }

        if (products.Count == 0)
        {
            ViewBag.FilterMessage = "Список товаров пуст";
        }

        List<Brand> brands = _db.Brands.ToList();
        ViewBag.Brands = brands;
        List<Category> categories = _db.Categories.ToList();
        ViewBag.Categories = categories;

        return View(products);
    }
    
    
    //----Добавление товара-[HttpGet]----------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Add()
    {
        List<Brand> brands = _db.Brands.ToList();
        ViewBag.Brands = new SelectList(brands, nameof(Brand.Id), nameof(Brand.BrandName));
        List<Category> categories = _db.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, nameof(Category.Id), nameof(Category.CategoryName));
        return View();
    }
    

    //----Добавление товара-[HttpPost]----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Add(Product product)
    {
        if (product.AvatarFile != null && product.AvatarFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.AvatarFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                product.AvatarFile.CopyTo(stream);
            }

            product.AvatarFileName = uniqueFileName;
        }
        else
        {
            product.AvatarFileName =
                "3ee29ee9-9bbc-4c13-86e9-1a763a10fce4_360_F_470299797_UD0eoVMMSUbHCcNJCdv2t8B2g1GVqYgs.jpg";
        }

        product.Creation = DateTime.Now;
        product.ProductId = Guid.NewGuid();
        _db.Products.Add(product);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
    

    //----Детальная страничка товара- [HttpGet]-------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Details(int id)
    {
        var product = _db.Products.Include(p => p.Brand).Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }
    

    //----Удаление товара- [HttpGet]------------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var product = _db.Products.FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }

        _db.Products.Remove(product);
        _db.SaveChanges();

        return RedirectToAction("Index");
        ;
    }
    

    //----Редактирования товара-[HttpGet]-------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        List<Brand> brands = _db.Brands.ToList();
        ViewBag.Brands = new SelectList(brands, nameof(Brand.Id), nameof(Brand.BrandName));
        List<Category> categories = _db.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, nameof(Category.Id), nameof(Category.CategoryName));
        var product = _db.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    
    
    //----Редактирования товара-[HttpPost]------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (product.AvatarFile != null && product.AvatarFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.AvatarFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                product.AvatarFile.CopyTo(stream);
            }

            product.AvatarFileName = uniqueFileName;
        }
        else
        {
            var productObject = _db.Products.AsNoTracking()
                .Where(p => p.Id == product.Id)
                .Select(p => p.AvatarFileName)
                .FirstOrDefault().ToString();

            product.AvatarFileName = productObject;
        }

        product.Update = DateTime.Now;
        _db.Update(product);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    
    //----Сортировка товара-[HttpGet]-----------------------------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> SortProduct(SortState state = SortState.NameAsc)
    {
        IQueryable<Product> products = _db.Products.Include(p => p.Brand).Include(p => p.Category);
        ViewBag.NameSort = state == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
        ViewBag.BrandSortSort = state == SortState.BrandAsc ? SortState.BrandDesc : SortState.BrandAsc;
        ViewBag.CreationSort = state == SortState.CreationAsc ? SortState.CreationDesc : SortState.CreationAsc;
        ViewBag.CategorySort = state == SortState.CategoryAsc ? SortState.CategoryDesc : SortState.CategoryAsc;
        ViewBag.PriceSort = state == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;

        switch (state)
        {
            case SortState.NameDesc:
                products = products.OrderByDescending(p => p.Name);
                break;
            case SortState.BrandAsc:
                products = products.OrderBy(p => p.Brand.BrandName);
                break;
            case SortState.BrandDesc:
                products = products.OrderByDescending(p => p.Brand.BrandName);
                break;
            case SortState.CreationAsc:
                products = products.OrderBy(p => p.Creation);
                break;
            case SortState.CreationDesc:
                products = products.OrderByDescending(p => p.Creation);
                break;
            case SortState.CategoryAsc:
                products = products.OrderBy(p => p.Category.CategoryName);
                break;
            case SortState.CategoryDesc:
                products = products.OrderByDescending(p => p.Category.CategoryName);
                break;
            case SortState.PriceAsc:
                products = products.OrderBy(p => p.Price);
                break;
            case SortState.PriceDesc:
                products = products.OrderByDescending(p => p.Price);
                break;
            default:
                products = products.OrderBy(p => p.Name);
                break;
        }
        return View(await products.AsNoTracking().ToListAsync());
    }
}