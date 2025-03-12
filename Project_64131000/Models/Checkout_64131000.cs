// File: Checkout_64131000.cs
using System.Collections.Generic;

namespace Project_64131000.Models
{
    public class Checkout_64131000
    {
        public List<ShoppingCart> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public string ShippingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }

        public Checkout_64131000()
        {
            CartItems = new List<ShoppingCart>();
        }
    }
}
