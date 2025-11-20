namespace FoodDeliveryApp.Models
{
    public class CheckoutModel
    {
        // Delivery & Billing
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;

        // Payment details
        public string PaymentMethod { get; set; } = "Card";
        public string CardName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string Expiry { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
        public string UpiId { get; set; } = string.Empty;
    }
}
