namespace FoodDeliveryApp.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string Address { get; set; }

        // Navigation property
        public List<MenuItem> MenuItems { get; set; } = new();
    }
}

