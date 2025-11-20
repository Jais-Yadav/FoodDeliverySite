using Microsoft.AspNetCore.Mvc;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Controllers
{
    public class OrdersController : Controller
    {
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                // Simulate order success
                return RedirectToAction("Success");
            }
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
