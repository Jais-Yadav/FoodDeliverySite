using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDistributedMemoryCache(); // In-memory cache for session storage
builder.Services.AddSession();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();

    if (!db.Restaurants.Any())
    {
        var r1 = new Restaurant
        {
            Name = "Spice Villa",
            Description = "Authentic Indian Cuisine",
            Address = "Mumbai, India",
            MenuItems = new List<MenuItem>
            {
                new MenuItem { Name = "Paneer Butter Masala", Price = 250, ImageUrl = "/images/paneer.jpg" },
                new MenuItem { Name = "Butter Naan", Price = 60, ImageUrl = "/images/naan.jpg" },
            }
        };

        var r2 = new Restaurant
        {
            Name = "Pizza Planet",
            Description = "Delicious handcrafted pizzas",
            Address = "Pune, India",
            MenuItems = new List<MenuItem>
            {
                new MenuItem { Name = "Margherita Pizza", Price = 300, ImageUrl = "/images/pizza.jpg" },
                new MenuItem { Name = "Veggie Delight", Price = 350, ImageUrl = "/images/veggie.jpg" },
                new MenuItem {Name="Burger",Price=50,ImageUrl="/images/burger.jpg"}
            }
        };

        db.Restaurants.AddRange(r1, r2);
        db.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication(); // ? Add this
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();
