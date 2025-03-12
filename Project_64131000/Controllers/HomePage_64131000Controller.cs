using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Project_64131000.Models;

namespace Project_64131000.Controllers
{
    public class HomePage_64131000Controller : Controller
    {
        private readonly Project_64131000Entities _context;

        public HomePage_64131000Controller()
        {
            _context = new Project_64131000Entities(); // Khởi tạo context
        }

        // Trang chủ hiển thị danh sách sản phẩm (sắp xếp theo CreateDate)
        public ActionResult Index()
        {
            var products = _context.Products
                                   .OrderByDescending(p => p.CreateDate) // Sắp xếp theo ngày tạo mới nhất
                                   .ToList();
            return View(products);
        }

        // Chi tiết sản phẩm
        public ActionResult ProductDetails(string id)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public ActionResult AddToCart(string id)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }

            // Lấy giỏ hàng từ session hoặc tạo mới
            var cartItems = (List<Cart_64131000>)Session["Cart"] ?? new List<Cart_64131000>();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            var cartItem = cartItems.SingleOrDefault(c => c.ProductId == id);
            if (cartItem != null)
            {
                cartItem.Quantity++; // Tăng số lượng
            }
            else
            {
                cartItems.Add(new Cart_64131000
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            Session["Cart"] = cartItems; // Cập nhật lại session
            return Json(new { success = true, message = "Đã thêm sản phẩm vào giỏ hàng!" });
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public ActionResult RemoveFromCart(string id)
        {
            var cartItems = (List<Cart_64131000>)Session["Cart"] ?? new List<Cart_64131000>();
            var cartItem = cartItems.SingleOrDefault(c => c.ProductId == id);

            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
                Session["Cart"] = cartItems;
                return Json(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng!" });
            }

            return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
        }
        // Xem thông tin cá nhân
        public ActionResult Profile()
        {
            var userId = Session["UserID"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account_64131000");
            }

            // Lấy email từ session trước
            var userEmail = Session["UserEmail"]?.ToString();
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account_64131000");
            }

            // Sử dụng email đã lấy để tìm người dùng
            var user = _context.Customers.SingleOrDefault(c => c.Email == userEmail);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
        // Chỉnh sửa thông tin người dùng
        public ActionResult Edit(string email)
        {
            var user = _context.Customers.SingleOrDefault(c => c.Email == email);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(Customers model)
        {
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.SingleOrDefault(c => c.Email == model.Email);
                var user = _context.Users.SingleOrDefault(c => c.Email == model.Email);

                if (customer != null)
                {
                    customer.FullName = model.FullName;
                    customer.PhoneNumber = model.PhoneNumber;
                    customer.CustomerAddress = model.CustomerAddress;
                    customer.PasswordHash = model.PasswordHash;
                    user.PasswordHash = model.PasswordHash;

                    _context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    return RedirectToAction("Index"); // Hoặc trang bạn muốn chuyển hướng
                }
            }
            return View(model); // Nếu không hợp lệ, quay lại trang chỉnh sửa
        }
        // Trang thanh toán
        public ActionResult Checkout()
        {
            var userId = Session["UserID"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account_64131000");
            }

            var cartItems = _context.ShoppingCart
                                    .Where(c => c.UserId == userId)
                                    .ToList();

            // Tính tổng số tiền
            decimal totalAmount = 0;
            foreach (var item in cartItems)
            {
                var product = _context.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    totalAmount += product.Price * (item.Quantity ?? 0);
                }
            }

            var checkoutViewModel = new Checkout_64131000
            {
                CartItems = cartItems,
                TotalAmount = totalAmount,
                UserId = userId
            };

            return View(checkoutViewModel);
        }

        // Xử lý thanh toán
        [HttpPost]
        public ActionResult ProcessCheckout(Checkout_64131000 model)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng từ session
                var userId = Session["UserID"]?.ToString();

                // Tạo đơn hàng mới
                var order = new Orders
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.Now,
                    ShipDate = DateTime.Now.AddDays(3), // Giao hàng sau 3 ngày
                    OrderStatus = "Đang Xử Lý",
                    CustomerId = userId,
                    TotalAmount = model.TotalAmount
                };

                // Thêm chi tiết đơn hàng
                foreach (var item in model.CartItems)
                {
                    order.OrderDetails.Add(new OrderDetails
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity ?? 0,
                        Price = item.Products.Price
                    });
                }

                // Lưu đơn hàng vào cơ sở dữ liệu
                try
                {
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            // Ghi lại hoặc hiển thị thông báo lỗi
                            ModelState.AddModelError("", validationError.ErrorMessage);
                        }
                    }
                    return View("Success", model); // Quay lại trang thanh toán với thông báo lỗi
                }

                // Xóa giỏ hàng sau khi thanh toán
                Session["Cart"] = null;

                return RedirectToAction("Success"); // Chuyển hướng tới trang thanh toán thành công
            }

            return View("Success", model); // Nếu không hợp lệ, hiển thị lại trang thanh toán
        }

        // Trang thành công
        public ActionResult Success()
        {
            return View();
        }

    }
}
