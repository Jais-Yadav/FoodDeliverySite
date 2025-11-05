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

        // Home page - list menu items
        public IActionResult Index()
        {
            var menuItems = _context.MenuItems.Include(m => m.Restaurant).ToList();
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
