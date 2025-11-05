namespace FoodDeliveryApp.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        // Foreign key
        public int RestaurantId { get; set; }

        // Navigation property
        public Restaurant? Restaurant { get; set; }
    }
}

