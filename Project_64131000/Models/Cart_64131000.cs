namespace Project_64131000.Models
{
    public class Cart_64131000
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price; // Tính tổng giá tiền
    }
}
