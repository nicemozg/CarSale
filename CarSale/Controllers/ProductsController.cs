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
            .Include(p => p.ModelAuto)
            .Include(p => p.ColorAuto)
            .ToList();

        List<Brand> brands = _db.Brands.ToList();
        ViewBag.Brands = brands;
        List<ModelAuto> carModels = _db.ModelAutos.ToList();
        ViewBag.Models = carModels;
        List<TypeMotor> carMotors = _db.TypeAutosMototors.ToList();
        ViewBag.Motors = carMotors;
        List<Category> categories = _db.Categories.ToList();
        ViewBag.Categories = categories;

        return View(products);
    }

    
    //----Страничка Index--[HttpPost]-----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Index(int? category, int? brand, int? model, int? motor)
    {
        List<Product> products = _db.Products
            .Include(p=>p.Category)
            .Include(p => p.Brand)
            .Include(p=>p.ModelAuto)
            .Include(p=>p.TypeMotor)
            .Include(p=>p.ColorAuto)
            .ToList();

        if (category.HasValue)
        {
            products = products.Where(p => p.Category.Id == category.Value).ToList();
        }

        if (brand.HasValue)
        {
            products = products.Where(p => p.Brand.Id == brand.Value).ToList();
        }
        
        if (model.HasValue)
        {
            products = products.Where(p => p.ModelAuto.Id == model.Value).ToList();
        }
        
        if (motor.HasValue)
        {
            products = products.Where(p => p.TypeMotor.Id == motor.Value).ToList();
        }

        if (products.Count == 0)
        {
            ViewBag.FilterMessage = "Такой автомобиль не найден";
        }

        List<Brand> brands = _db.Brands.ToList();
        ViewBag.Brands = brands;
        List<ModelAuto> carModels = _db.ModelAutos.ToList();
        ViewBag.Models = carModels;
        List<TypeMotor> carMotors = _db.TypeAutosMototors.ToList();
        ViewBag.Motors = carMotors;
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
        List<ModelAuto> models = _db.ModelAutos.ToList();
        ViewBag.Models = new SelectList(models, nameof(ModelAuto.Id), nameof(ModelAuto.ModelName));
        List<Category> categories = _db.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, nameof(Category.Id), nameof(Category.CategoryName));
        List<TypeAuto> typeAutos = _db.TypeAutos.ToList();
        ViewBag.TypesAuto = new SelectList(typeAutos, nameof(TypeAuto.Id), nameof(TypeAuto.TypeName));
        List<TypeMotor> typeMotors = _db.TypeAutosMototors.ToList();
        ViewBag.TypesMotor = new SelectList(typeMotors, nameof(TypeMotor.Id), nameof(TypeMotor.TypeName));
        List<ColorAuto> colors = _db.ColorsAutos.ToList();
        ViewBag.Colors = new SelectList(colors, nameof(ColorAuto.Id), nameof(ColorAuto.ColorName));
        return View();
    }
    

    //----Добавление товара-[HttpPost]----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Add(Product product)
    {
        try
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (product.AvatarFile != null && product.AvatarFile.Length > 0)
            {
                uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
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
            //----------------------------------------------------------------------------------------------------------
            if (product.InfoFile != null && product.InfoFile.Length > 0)
            {
                uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.InfoFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    product.InfoFile.CopyTo(stream);
                }

                product.InfoFileName = uniqueFileName;
            }
            else
            {
                product.InfoFileName =
                    "3ee29ee9-9bbc-4c13-86e9-1a763a10fce4_360_F_470299797_UD0eoVMMSUbHCcNJCdv2t8B2g1GVqYgs.jpg";
            }
            //----------------------------------------------------------------------------------------------------------
            
            product.Creation = DateTime.Now;
            if (product.SteeringWheel == "right")
            {
                product.SteeringWheel = "Справа";
            }
            else
            {
                product.SteeringWheel = "Слева";
            }

            if (product.Transmission == "auto")
            {
                product.Transmission = "АКПП";
            }
            else if (product.Transmission == "robot")
            {
                product.Transmission = "Робот";
            }
            else
            {
                product.Transmission = "МКПП";
            }
            
            // Добавляем продукт в базу данных
            _db.Products.Add(product);
            _db.SaveChanges();

            // Теперь product.Id содержит уникальный идентификатор продукта

            // Добавление фотографий в коллекцию Photos у продукта
            foreach (var photoFile in product.PhotoFiles)
            {
                if (photoFile != null && photoFile.Length > 0)
                {
                    string uniqueFileName = Path.GetRandomFileName() + Path.GetExtension(photoFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        photoFile.CopyTo(stream);
                    }

                    Photos photo = new Photos
                    {
                        PhotoPath = uniqueFileName,
                        ProductId = product.Id
                    };

                    _db.Photos.Add(photo);
                }
            }

            _db.SaveChanges(); // Сохраняем изменения в базе данных

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Произошла ошибка при добавлении продукта: {ex.Message}");
            return View(product);
        }
    }


    
    //----Детальная страничка товара- [HttpGet]-------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Details(int id)
    {
        var product = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.ModelAuto)
            .Include(p => p.Category)
            .Include(p=>p.TypeAuto)
            .Include(p=>p.TypeMotor)
            .Include(p=>p.ColorAuto)
            .FirstOrDefault(p => p.Id == id);

        if (product is null)
        {
            return NotFound();
        }
        
        product.Photos = _db.Photos.Where(photo => photo.ProductId == id).ToList();

        return View(product);
    }

    
    //----Удаление товара- [HttpGet]------------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var product = _db.Products
            .FirstOrDefault(p => p.Id == id);

        if (product is null)
        {
            return NotFound();
        }

        // Remove photos based on a condition (example: remove photos with a specific property)
        var photosToRemove = _db.Photos.Where(photo => photo.ProductId == product.Id).ToList();
        _db.Photos.RemoveRange(photosToRemove);

        _db.Products.Remove(product);
        _db.SaveChanges();

        return RedirectToAction("Index");
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
                products = products.OrderByDescending(p => p.ModelAuto.ModelName);
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
                products = products.OrderBy(p => p.ModelAuto.ModelName);
                break;
        }
        return View(await products.AsNoTracking().ToListAsync());
    }
}