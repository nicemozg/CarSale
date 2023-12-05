using CarSale.Context;
using CarSale.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSale.Controllers;

public class СategoriesController : Controller
{
    private Car_Sale_Context _db;
    private readonly UserManager<User> _userManager;

    public СategoriesController(Car_Sale_Context db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    
    //----Страничка Index категорий--[HttpGet]--------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Index()
    {
        List <Category> categories = _db.Categories.ToList();
        return View(categories);
    }
    
    //----Добавление категорий--[HttpGet]-------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    //----Добавление категорий--[HttpPost]------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Add(Category? model)
    {
        if (ModelState.IsValid)
        {
            if (_db.Categories.Any(b=>b.CategoryName==model.CategoryName))
            {
                ViewBag.ErrorExist = "Категория уже существует";
                return View();
            }
            _db.Categories.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View();
    }
    
    //----Удаление категорий--[HttpGet]---------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var category = _db.Categories.FirstOrDefault(c => c.Id == id);
        if (category is null)
        {
            return NotFound();
        }

        _db.Categories.Remove(category);
        _db.SaveChanges();

        return RedirectToAction("Index");;
    }
    
    //----Редактирвание категорий--[HttpGet]----------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var category = _db.Categories.FirstOrDefault(c => c.Id == id);
        return View(category);
    }

    //----Редактирование категорий--[HttpGet]---------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Edit(Category category)
    {
        _db.Categories.Update(category);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}