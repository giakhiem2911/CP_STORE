using System;
using System.Linq;
using System.Web.Mvc;
using Project_64131000.Models;

namespace Project_64131000.Controllers
{
    public class Account_64131000Controller : Controller
    {
        private Project_64131000Entities db = new Project_64131000Entities();

        // GET: Account_64131000/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account_64131000/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users user)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = db.Users.FirstOrDefault(u => u.Username == user.Username && u.PasswordHash == user.PasswordHash);
                if (loggedInUser != null)
                {
                    // Lưu thông tin vào session
                    Session["UserID"] = loggedInUser.UserId;
                    Session["Role"] = loggedInUser.Role;
                    Session["UserEmail"] = loggedInUser.Email;

                    // Chuyển hướng theo vai trò
                    return RedirectToAction(loggedInUser.Role == "Employee" ? "Index" : "Index", loggedInUser.Role == "Employee" ? "Products_64131000" : "HomePage_64131000");
                }
                ModelState.AddModelError("", "Tên người dùng hoặc mật khẩu không đúng.");
            }
            return View(user);
        }
        // GET: Account_64131000/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account_64131000/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Users user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem username đã tồn tại chưa
                var existingUser = db.Users.FirstOrDefault(u => u.Username == user.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                    return View(user);
                }

                // Đặt role mặc định cho người dùng
                user.Role = "Customer";
                db.Users.Add(user);
                db.SaveChanges();

                // Tạo đối tượng Customer và thêm vào bảng Customers
                var customer = new Customers
                {

                    FullName = user.Username,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email
                };
                // Chuyển hướng về trang đăng nhập sau khi đăng ký thành công
                return RedirectToAction("Login", "Account_64131000");
            }
            return View(user);
        }

        // GET: Account_64131000/Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa session khi đăng xuất
            return RedirectToAction("Login");
        }
    }
}
