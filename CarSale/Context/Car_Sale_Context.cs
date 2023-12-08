using CarSale.Controllers;
using CarSale.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarSale.Context
{
    public class Car_Sale_Context : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Photos> Photos { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TypeAuto> TypeAutos { get; set; }
        public DbSet<TypeMotor> TypeAutosMototors { get; set; }
        public DbSet<ModelAuto> ModelAutos { get; set; }
        public DbSet<ColorAuto> ColorsAutos { get; set; }
        
        public Car_Sale_Context(DbContextOptions<Car_Sale_Context> options) : base(options)
        {
        }
    }
}

