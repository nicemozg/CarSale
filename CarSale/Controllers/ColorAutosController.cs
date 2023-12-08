using CarSale.Context;
using CarSale.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSale.Controllers;

public class ColorAutosController : Controller
{
    
    private Car_Sale_Context _db;
    private readonly UserManager<User> _userManager;

    public ColorAutosController(Car_Sale_Context db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        List <ColorAuto> colors = _db.ColorsAutos.ToList();
        return View(colors);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    
    //----Добавление брэнда-[HttpPost]----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Add(ColorAuto model)
    {
        if (ModelState.IsValid)
        {
            if (_db.ColorsAutos.Any(c=>c.ColorName==model.ColorName))
            {
                ViewBag.ErrorExist = "Цвет уже существует";
                return View();
            }
            _db.ColorsAutos.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View();
    }
    
    //----Редактирование брэнда--[HttpGet]--------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var сolor = _db.ColorsAutos.FirstOrDefault(с => с.Id == id);
        return View(сolor);
    }
    
    //----Редактирование брэнда--[HttpPost]-------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Edit(ColorAuto model)
    {
        if (ModelState.IsValid)
        {
            _db.ColorsAutos.Update(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        return View();
    }
    
    //----Удаление брэнда--[HttpGet]--------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var color = _db.ColorsAutos.FirstOrDefault(c => c.Id == id);
        if (color is null)
        {
            return NotFound();
        }

        _db.ColorsAutos.Remove(color);
        _db.SaveChanges();

        return RedirectToAction("Index");;
    }
}