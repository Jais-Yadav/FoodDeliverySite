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
// Add both MVC and Razor Pages support
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
                new MenuItem { Name = "Paneer Butter Masala", Price = 250, ImageUrl = "/images/paneer.jpg", Category = "Lunch" },
                new MenuItem { Name = "Butter Naan", Price = 60, ImageUrl = "/images/naan.jpg", Category = "Lunch" },
            }
        };

        var r2 = new Restaurant
        {
            Name = "Pizza Planet",
            Description = "Delicious handcrafted pizzas",
            Address = "Pune, India",
            MenuItems = new List<MenuItem>
            {
                new MenuItem { Name = "Margherita Pizza", Price = 300, ImageUrl = "/images/pizza.jpg", Category = "Fast Food" },
                new MenuItem { Name = "Veggie Delight", Price = 350, ImageUrl = "/images/veggie.jpg", Category = "Fast Food" },
                new MenuItem { Name = "Burger", Price = 50, ImageUrl = "/images/burger.jpg", Category = "Fast Food" }
            }
        };

        var r3 = new Restaurant
        {
            Name = "Brew & Chill",
            Description = "Hot and cold beverages",
            Address = "Pune, India",
            MenuItems = new List<MenuItem>
            {
                new MenuItem { Name = "Cold Coffee", Price = 80, ImageUrl = "/images/coffee.jpg", Category = "Beverages" },
                new MenuItem { Name = "Masala Chai", Price = 40, ImageUrl = "/images/chai.jpg", Category = "Beverages" }
            }
        };

        db.Restaurants.AddRange(r1, r2, r3);
        db.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// keep controllers routes and enable Razor Pages
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
