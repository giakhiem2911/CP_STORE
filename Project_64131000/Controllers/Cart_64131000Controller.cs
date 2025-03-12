using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Project_64131000.Models;

namespace Project_64131000.Controllers
{
    public class Cart_64131000Controller : Controller
    {
        private const string ConnectionString = "MSI"; // Cập nhật chuỗi kết nối của bạn

        [HttpPost]
        public JsonResult AddToCart(string productId, string userId)
        {
            try
            {
                // Kiểm tra sản phẩm có tồn tại không (có thể gọi đến một phương thức để kiểm tra)
                if (GetProductById(productId) == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại." });
                }

                // Thêm sản phẩm vào giỏ hàng
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
                    var command = new SqlCommand("SELECT Quantity FROM ShoppingCart WHERE ProductId = @ProductId AND UserId = @UserId", connection);
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@UserId", userId);
                    var existingQuantity = command.ExecuteScalar() as int?;

                    if (existingQuantity.HasValue)
                    {
                        // Nếu đã có, cập nhật số lượng
                        command = new SqlCommand("UPDATE ShoppingCart SET Quantity = Quantity + 1 WHERE ProductId = @ProductId AND UserId = @UserId", connection);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        // Nếu chưa có, thêm mới
                        var cartId = Guid.NewGuid().ToString();
                        command = new SqlCommand("INSERT INTO ShoppingCart (CartId, UserId, ProductId, Quantity) VALUES (@CartId, @UserId, @ProductId, @Quantity)", connection);
                        command.Parameters.AddWithValue("@CartId", cartId);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@Quantity", 1);
                        command.ExecuteNonQuery();
                    }
                }

                return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }

        private Products GetProductById(string productId)
        {
            // Hàm này nên được thực hiện để lấy sản phẩm từ bảng Products
            // Tạm thời sử dụng null cho ví dụ
            return null;
        }

        public ActionResult Index(string userId)
        {
            // Lấy giỏ hàng của người dùng
            var cart = new List<ShoppingCart>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM ShoppingCart WHERE UserId = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    cart.Add(new ShoppingCart
                    {
                        CartId = reader["CartId"].ToString(),
                        UserId = reader["UserId"].ToString(),
                        ProductId = reader["ProductId"].ToString(),
                        Quantity = (int)reader["Quantity"],
                        CreatedAt = (DateTime)reader["CreatedAt"]
                    });
                }
            }
            return View(cart);
        }
    }
}
