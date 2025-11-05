using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _context.CartItems.Include(c => c.MenuItem).ToListAsync();
            ViewBag.Total = cart.Sum(i => i.MenuItem.Price * i.Quantity);
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            var existing = await _context.CartItems.FirstOrDefaultAsync(c => c.MenuItemId == id);

            if (existing != null)
                existing.Quantity++;
            else
                _context.CartItems.Add(new CartItem { MenuItemId = id, Quantity = 1 });

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null) _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                item.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            _context.CartItems.RemoveRange(_context.CartItems);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
