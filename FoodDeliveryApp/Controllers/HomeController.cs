using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Home page - list menu items with optional category filter and search
        public IActionResult Index(string? category, string? q)
        {
            var query = _context.MenuItems.Include(m => m.Restaurant).AsQueryable();

            // Expose category list and current selections to the view
            ViewData["Categories"] = new List<string> { "All", "Fast Food", "Beverages", "Lunch" };
            ViewData["SelectedCategory"] = string.IsNullOrEmpty(category) ? "All" : category!;
            ViewData["SearchQuery"] = q ?? string.Empty;

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                query = query.Where(m => m.Category == category);
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                var search = q.Trim();
                query = query.Where(m =>
                    EF.Functions.Like(m.Name, $"%{search}%") ||
                    (m.Restaurant != null && EF.Functions.Like(m.Restaurant.Name, $"%{search}%"))
                );
            }

            var menuItems = query.ToList();
            return View(menuItems);
        }

        // Add to cart
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var item = _context.MenuItems.FirstOrDefault(m => m.Id == id);
            if (item == null)
                return NotFound();

            var cartItem = _context.CartItems.FirstOrDefault(c => c.MenuItemId == id);
            if (cartItem == null)
            {
                cartItem = new CartItem { MenuItemId = id, Quantity = 1 };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += 1;
                _context.CartItems.Update(cartItem);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // View Cart
        public IActionResult Cart()
        {
            var cartItems = _context.CartItems
                .Include(c => c.MenuItem)
                .ThenInclude(m => m.Restaurant)
                .ToList();
            return View(cartItems);
        }
    }
}
