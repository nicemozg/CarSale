using CarSale.Context;
using CarSale.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSale.Controllers;

public class TypeAutosController : Controller
{
    private Car_Sale_Context _db;
    private readonly UserManager<User> _userManager;

    public TypeAutosController(Car_Sale_Context db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        List <TypeAuto> typeAutos = _db.TypeAutos.ToList();
        return View(typeAutos);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    
    //----Добавление брэнда-[HttpPost]----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Add(TypeAuto model)
    {
        if (ModelState.IsValid)
        {
            if (_db.TypeAutos.Any(b=>b.TypeName==model.TypeName))
            {
                ViewBag.ErrorExist = "Такой тип кузова уже существует";
                return View();
            }
            _db.TypeAutos.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View();
    }
    
    //----Редактирование брэнда--[HttpGet]--------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var typeAuto = _db.TypeAutos.FirstOrDefault(b => b.Id == id);
        return View(typeAuto);
    }
    
    //----Редактирование брэнда--[HttpPost]-------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Edit(TypeAuto model)
    {
        if (ModelState.IsValid)
        {
            _db.TypeAutos.Update(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        return View();
    }
    
    //----Удаление брэнда--[HttpGet]--------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var typeAuto = _db.TypeAutos.FirstOrDefault(t => t.Id == id);
        if (typeAuto is null)
        {
            return NotFound();
        }

        _db.TypeAutos.Remove(typeAuto);
        _db.SaveChanges();

        return RedirectToAction("Index");;
    }
}