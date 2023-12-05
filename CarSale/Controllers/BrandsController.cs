using CarSale.Context;
using CarSale.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSale.Controllers;

public class BrandsController : Controller
{
    private Car_Sale_Context _db;
    private readonly UserManager<User> _userManager;

    public BrandsController(Car_Sale_Context db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // GET
    [HttpGet]
    public IActionResult Index()
    {
        List <Brand> brands = _db.Brands.ToList();
        return View(brands);
    }
    
      [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    
    //----Добавление брэнда-[HttpPost]----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Add(Brand model)
    {
        if (ModelState.IsValid)
        {
            if (_db.Brands.Any(b=>b.BrandName==model.BrandName))
            {
                ViewBag.ErrorExist = "Брэнд уже существует";
                return View();
            }
            _db.Brands.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View();
    }
    
    //----Редактирование брэнда--[HttpGet]--------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var brand = _db.Brands.FirstOrDefault(b => b.Id == id);
        return View(brand);
    }
    
    //----Редактирование брэнда--[HttpPost]-------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Edit(Brand brand)
    {
        if (ModelState.IsValid)
        {
                _db.Brands.Update(brand);
                _db.SaveChanges();
                return RedirectToAction("Index");
        }
        
        return View();
    }
    
    //----Удаление брэнда--[HttpGet]--------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var brand = _db.Brands.FirstOrDefault(b => b.Id == id);
        if (brand is null)
        {
            return NotFound();
        }

        _db.Brands.Remove(brand);
        _db.SaveChanges();

        return RedirectToAction("Index");;
    }
}