using CarSale.Context;
using CarSale.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSale.Controllers;

public class ModelAutosController : Controller
{
    private Car_Sale_Context _db;
    private readonly UserManager<User> _userManager;

    public ModelAutosController(Car_Sale_Context db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    // GET
    [HttpGet]
    public IActionResult Index()
    {
        List <ModelAuto> brands = _db.ModelAutos.ToList();
        return View(brands);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    
    //----Добавление брэнда-[HttpPost]----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Add(ModelAuto model)
    {
        if (ModelState.IsValid)
        {
            if (_db.ModelAutos.Any(b=>b.ModelName==model.ModelName))
            {
                ViewBag.ErrorExist = "Модель уже существует";
                return View();
            }
            _db.ModelAutos.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View();
    }
    
    //----Редактирование брэнда--[HttpGet]--------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var modelAuto = _db.ModelAutos.FirstOrDefault(m => m.Id == id);
        return View(modelAuto);
    }
    
    //----Редактирование брэнда--[HttpPost]-------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Edit(ModelAuto model)
    {
        if (ModelState.IsValid)
        {
            _db.ModelAutos.Update(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        return View();
    }
    
    //----Удаление брэнда--[HttpGet]--------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var modelAuto = _db.ModelAutos.FirstOrDefault(m => m.Id == id);
        if (modelAuto is null)
        {
            return NotFound();
        }

        _db.ModelAutos.Remove(modelAuto);
        _db.SaveChanges();

        return RedirectToAction("Index");;
    }
}