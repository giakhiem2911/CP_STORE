using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Project_64131000.Models;

namespace Project_64131000.Controllers
{
    public class ShoppingCarts_64131000Controller : Controller
    {
        private Project_64131000Entities db = new Project_64131000Entities();

        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            string userId = Session["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account_64131000");
            }

            var cartItems = db.ShoppingCart.Where(c => c.UserId == userId).ToList();
            return View(cartItems);
        }

        [HttpPost]
        public ActionResult AddToCart(string productId, int quantity)
        {
            string userId = Session["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng." });
            }

            var product = db.Products.Find(productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            try
            {
                var existingCartItem = db.ShoppingCart.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity; // Cập nhật số lượng nếu sản phẩm đã có trong giỏ
                }
                else
                {
                    var newCartItem = new ShoppingCart
                    {
                        CartId = Guid.NewGuid().ToString().Substring(0, 20), // Tạo CartId mới
                        UserId = userId,
                        ProductId = productId,
                        Quantity = quantity,
                        CreatedAt = DateTime.Now
                    };
                    db.ShoppingCart.Add(newCartItem); // Thêm sản phẩm mới vào giỏ hàng
                }

                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng." });
            }
            catch (DbEntityValidationException dbEx)
            {
                var errors = dbEx.EntityValidationErrors.SelectMany(e => e.ValidationErrors)
                    .Select(e => e.ErrorMessage).ToList();

                // Ghi lại chi tiết lỗi vào log để kiểm tra
                System.Diagnostics.Debug.WriteLine("Validation Errors: " + string.Join(", ", errors));

                return Json(new { success = false, message = "Có lỗi xảy ra: " + string.Join(", ", errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
            }
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cartItem = db.ShoppingCart.FirstOrDefault(c => c.CartId == id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }

            return View(cartItem); // Trả về View với đối tượng ShoppingCart cần xóa
        }

        // POST: ShoppingCarts_64131000/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cartItem = db.ShoppingCart.FirstOrDefault(c => c.CartId == id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.ShoppingCart.Remove(cartItem); // Xóa mục giỏ hàng
                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction("Index"); // Quay lại trang danh sách giỏ hàng
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Không thể xóa mục giỏ hàng: " + ex.Message);
                return View(cartItem);
            }
        }
    }
}
