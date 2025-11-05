using Microsoft.AspNetCore.Identity;

namespace FoodDeliveryApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}
