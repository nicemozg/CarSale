using CarSale.Context;
using CarSale.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarSale.Controllers;

public class TypeMotorsController : Controller
{
    private Car_Sale_Context _db;
    private readonly UserManager<User> _userManager;

    public TypeMotorsController(Car_Sale_Context db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        List <TypeMotor> typeAutos = _db.TypeAutosMototors.ToList();
        return View(typeAutos);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    
    
    //----Добавление брэнда-[HttpPost]----------------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Add(TypeMotor model)
    {
        if (ModelState.IsValid)
        {
            if (_db.TypeAutosMototors.Any(b=>b.TypeName==model.TypeName))
            {
                ViewBag.ErrorExist = "Такой тип двгателя уже существует";
                return View();
            }
            _db.TypeAutosMototors.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View();
    }
    
    //----Редактирование брэнда--[HttpGet]--------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var typeAutoMotor = _db.TypeAutosMototors.FirstOrDefault(t => t.Id == id);
        return View(typeAutoMotor);
    }
    
    //----Редактирование брэнда--[HttpPost]-------------------------------------------------------------------------
    [HttpPost]
    public IActionResult Edit(TypeMotor model)
    {
        if (ModelState.IsValid)
        {
            _db.TypeAutosMototors.Update(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        return View();
    }
    
    //----Удаление брэнда--[HttpGet]--------------------------------------------------------------------------------
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var typeAuto = _db.TypeAutosMototors.FirstOrDefault(t => t.Id == id);
        if (typeAuto is null)
        {
            return NotFound();
        }

        _db.TypeAutosMototors.Remove(typeAuto);
        _db.SaveChanges();

        return RedirectToAction("Index");;
    }
}