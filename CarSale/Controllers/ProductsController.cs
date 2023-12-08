using System.Globalization;
using System.Xml.Linq;
using CarSale.Context;
using CarSale.Enums;
using CarSale.Models;
using CarSale.ViewModels;
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

    public ProductsController(Car_Sale_Context db, UserManager<User> userManager,
        IWebHostEnvironment hostingEnvironment)
    {
        _db = db;
        _userManager = userManager;
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
    {
        List<Product> products = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.ModelAuto)
            .Include(p => p.ColorAuto)
            .Include(p => p.TypeMotor)
            .ToList();
        var cookieId = GetOrCreateFilterCookieId(); // Реализуйте этот метод
        var currentFilter = _db.Filters.FirstOrDefault(f => f.CookieId == cookieId);
        if (currentFilter == null)
        {
            currentFilter = new Filter
            {
                CookieId = cookieId,
                CategoryId = 0,
                BrandId = 0,
                MotorId = 0
            };
        
            _db.Filters.Add(currentFilter);
            await _db.SaveChangesAsync();
        }
        else
        {
            if (currentFilter.CategoryId > 0 || currentFilter.BrandId > 0 || currentFilter.MotorId > 0)
            {
                if (currentFilter.CategoryId > 0)
                {
                    products = products.Where(p => p.Category.Id == currentFilter.CategoryId).ToList();
                }
        
                if (currentFilter.BrandId > 0)
                {
                    products = products.Where(p => p.Brand.Id == currentFilter.BrandId).ToList();
                }
        
                if (currentFilter.MotorId > 0)
                {
                    products = products.Where(p => p.TypeMotor.Id == currentFilter.MotorId).ToList();
                }
        
                if (products.Count == 0)
                {
                    ViewBag.FilterMessage = "Такой автомобиль не найден";
                }
            }
        }
        ViewBag.SelectedCategoryId = currentFilter.CategoryId;
        ViewBag.SelectedBrandId = currentFilter.BrandId;
        ViewBag.SelectedMotorId = currentFilter.MotorId;
        List<Brand> brands = _db.Brands.ToList();
        ViewBag.Brands = brands;
        List<ModelAuto> carModels = _db.ModelAutos.ToList();
        ViewBag.Models = carModels;
        List<TypeMotor> carMotors = _db.TypeAutosMototors.ToList();
        ViewBag.Motors = carMotors;
        List<Category> categories = _db.Categories.ToList();
        ViewBag.Categories = categories;
        
        decimal usdExchangeRate = await GetUsdExchangeRateAsync();
        foreach (var product in products)
        {
            product.Price *= usdExchangeRate;
        }
        var pagedThemes = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var paginationViewModel = new PaginationThemeViewModel
        {
            Products = pagedThemes,
            TotalCount = products.Count,
            CurrentPage = page,
            PageSize = pageSize
        };
        return View(paginationViewModel);
    }

    //----Страничка Index--[HttpPost]-----------------------------------------------------------------------------------
    [HttpPost]
    public async Task<IActionResult> Index(int? category, int? brand, int? model, int? motor)
    {
        var cookieId = GetOrCreateFilterCookieId(); // Реализуйте этот метод
        var currentFilter = _db.Filters.FirstOrDefault(f => f.CookieId == cookieId);
    
        if (currentFilter == null)
        {
            currentFilter = new Filter
            {
                CookieId = cookieId,
                CategoryId = 0,
                BrandId = 0,
                MotorId = 0
            };
        
            _db.Filters.Add(currentFilter);
        }
        
        currentFilter.CategoryId = category ?? 0;
        currentFilter.BrandId = brand ?? 0;
        currentFilter.MotorId = motor ?? 0;
        _db.Filters.Update(currentFilter);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
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
    public async Task<IActionResult> Details(int id)
    {
        decimal usdExchangeRate = await GetUsdExchangeRateAsync();
        var product = _db.Products
            .Include(p => p.Brand)
            .Include(p => p.ModelAuto)
            .Include(p => p.Category)
            .Include(p => p.TypeAuto)
            .Include(p => p.TypeMotor)
            .Include(p => p.ColorAuto)
            .FirstOrDefault(p => p.Id == id);

        if (product is null)
        {
            return NotFound();
        }

        product.Photos = _db.Photos.Where(photo => photo.ProductId == id).ToList();
        product.Price *= usdExchangeRate;

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

        // Удаление файла, связанного с AvatarFileName
        string avatarFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", product.AvatarFileName);
        if (System.IO.File.Exists(avatarFilePath))
        {
            System.IO.File.Delete(avatarFilePath);
        }

        // Удаление файла, связанного с InfoFileName
        string infoFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", product.InfoFileName);
        if (System.IO.File.Exists(infoFilePath))
        {
            System.IO.File.Delete(infoFilePath);
        }

        var photosToRemove = _db.Photos.Where(photo => photo.ProductId == product.Id).ToList();

        foreach (var photo in photosToRemove)
        {
            string photoFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", photo.PhotoPath);
            if (System.IO.File.Exists(photoFilePath))
            {
                System.IO.File.Delete(photoFilePath);
            }
        }

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
            string currentAvatarFileName = _db.Products.AsNoTracking()
                .Where(p => p.Id == product.Id)
                .Select(p => p.AvatarFileName)
                .FirstOrDefault()
                .ToString();

            product.AvatarFileName = UploadFile(product.AvatarFile, currentAvatarFileName, "uploads");
        }
        else
        {
            var productObject = _db.Products.AsNoTracking()
                .Where(p => p.Id == product.Id)
                .Select(p => p.AvatarFileName)
                .FirstOrDefault().ToString();

            product.AvatarFileName = productObject;
        }

        //--------------------------------------------------------------------------------------------------------------
        if (product.InfoFile != null && product.InfoFile.Length > 0)
        {
            string currentInfoFileName = _db.Products.AsNoTracking()
                .Where(p => p.Id == product.Id)
                .Select(p => p.InfoFileName)
                .FirstOrDefault()
                .ToString();

            product.InfoFileName = UploadFile(product.InfoFile, currentInfoFileName, "uploads");
        }
        else
        {
            var productObject = _db.Products.AsNoTracking()
                .Where(p => p.Id == product.Id)
                .Select(p => p.InfoFileName)
                .FirstOrDefault().ToString();

            product.InfoFileName = productObject;
        }

        if (product.PhotoFiles != null && product.PhotoFiles.Any())
        {
            List<string> currentPhotoFileNames = _db.Photos
                .Where(p => p.ProductId == product.Id)
                .Select(p => p.PhotoPath)
                .ToList();

            List<string> newPhotoFileNames = UploadFiles(product.PhotoFiles, currentPhotoFileNames, "uploads");
            for (int i = 0; i < newPhotoFileNames.Count; i++)
            {
                Photos photo = new Photos
                {
                    PhotoPath = newPhotoFileNames[i],
                    ProductId = product.Id
                };

                _db.Photos.Add(photo);
            }

            var photosToRemove = _db.Photos.Where(photo => photo.ProductId == product.Id).ToList();
            _db.Photos.RemoveRange(photosToRemove);
        }
        else
        {
            List<Photos> existingPhotos = _db.Photos
                .Where(p => p.ProductId == product.Id)
                .ToList();
            foreach (var existingPhoto in existingPhotos)
            {
                existingPhoto.PhotoPath = existingPhoto.PhotoPath;
                _db.Photos.Update(existingPhoto);
            }
        }

        //--------------------------------------------------------------------------------------------------------------
        if (product.SteeringWheel == "right")
            product.SteeringWheel = "Справа";
        else
            product.SteeringWheel = "Слева";
        if (product.Transmission == "auto")
            product.Transmission = "АКПП";
        else if (product.Transmission == "robot")
            product.Transmission = "Робот";
        else
            product.Transmission = "МКПП";
        product.Update = DateTime.Now;
        _db.Update(product);
        _db.SaveChanges();
        return RedirectToAction("Details", "Products", new { id = product.Id });
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

    //------------------------------------------------------------------------------------------------------------------
    private string UploadFile(IFormFile file, string currentFileName, string folderName)
    {
        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        if (!string.IsNullOrEmpty(currentFileName))
        {
            string currentFilePath = Path.Combine(uploadsFolder, currentFileName);

            if (System.IO.File.Exists(currentFilePath))
            {
                System.IO.File.Delete(currentFilePath);
            }
        }

        string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return uniqueFileName;
    }

    private List<string> UploadFiles(List<IFormFile> files, List<string> currentFileNames, string folderName)
    {
        List<string> newFileNames = new List<string>();
        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folderName);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Delete current files
        for (int i = 0; i < currentFileNames.Count; i++)
        {
            string currentFilePath = Path.Combine(uploadsFolder, currentFileNames[i]);

            if (System.IO.File.Exists(currentFilePath))
            {
                System.IO.File.Delete(currentFilePath);
            }
        }

        // Upload new files
        for (int i = 0; i < files.Count; i++)
        {
            if (files[i] != null && files[i].Length > 0)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + files[i].FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    files[i].CopyTo(stream);
                }

                newFileNames.Add(uniqueFileName);
            }
            else
            {
                newFileNames.Add(currentFileNames[i]);
            }
        }

        return newFileNames;
    }


    public async Task<decimal> GetUsdExchangeRateAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = "https://nationalbank.kz/rss/get_rates.cfm?fdate=" + DateTime.Now.ToString("dd.MM.yyyy");

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                // Извлечение курса доллара из XML-ответа
                decimal usdRate = ExtractUsdRateFromXml(content);
                return usdRate;
            }
            else
            {
                // Обработка ошибки
                return 0;
            }
        }
    }

    private decimal ExtractUsdRateFromXml(string xmlContent)
    {
        try
        {
            XDocument xmlDoc = XDocument.Parse(xmlContent);

            // Найти элемент "item" с fullname, содержащим "ДОЛЛАР США"
            XElement usdElement = xmlDoc.Descendants("item")
                .FirstOrDefault(item => item.Element("fullname")?.Value == "ДОЛЛАР США");
            if (usdElement != null)
            {
                // Извлечь значение курса доллара
                string usdRateStr = usdElement.Element("description")?.Value;
                usdRateStr = usdRateStr.Replace(" ", "");
                usdRateStr = new string(usdRateStr.Where(c => char.IsDigit(c) || c == '.').ToArray());
                Console.WriteLine($"After cleaning: '{usdRateStr}'");
                if (decimal.TryParse(usdRateStr, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal usdRate))
                {
                    return usdRate;
                }
            }
        }
        catch (Exception ex)
        {
            // Обработка ошибки парсинга XML
            Console.WriteLine($"Error extracting USD rate: {ex.Message}");
        }

        return 0;
    }
    
    private string GetOrCreateFilterCookieId()
    {
        string cookieId;
    
        // Попытка получить значение куки из запроса
        if (Request.Cookies.TryGetValue("FilterCookieId", out cookieId))
        {
            return cookieId;
        }
        else
        {
            // Если куки не существует, создаем новый идентификатор
            cookieId = Guid.NewGuid().ToString();

            // Установка значения куки в ответе
            Response.Cookies.Append("FilterCookieId", cookieId, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddYears(1) // Настройте срок действия куки по вашему усмотрению
            });

            return cookieId;
        }
    }

}