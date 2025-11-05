namespace FoodDeliveryApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }

        // Navigation property
        public MenuItem? MenuItem { get; set; }
    }
}

