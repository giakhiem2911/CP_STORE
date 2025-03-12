using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Project_64131000.Models;

namespace Project_64131000.Controllers
{
    public class Orders_64131000Controller : Controller
    {
        private Project_64131000Entities db = new Project_64131000Entities();

        // GET: Orders_64131000
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customers).Include(o => o.Employees);
            return View(orders.ToList());
        }

        // POST: Orders_64131000/ConfirmOrder/5
        [HttpPost]
        public ActionResult ConfirmOrder(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            order.OrderStatus = "Đã xác nhận"; // Cập nhật trạng thái đơn hàng
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Orders_64131000/Pay
        [HttpPost]
        public ActionResult Pay(string customerId, string employeeId)
        {
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(employeeId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var newOrder = new Orders
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now.AddDays(3), // Giả định ship hàng sau 3 ngày
                OrderStatus = "Đã thanh toán",
                CustomerId = customerId,
                EmployeeId = employeeId,
                TotalAmount = 0 // Bạn có thể tính toán tổng tiền từ giỏ hàng
            };

            db.Orders.Add(newOrder);
            db.SaveChanges();

            // Sau khi tạo hóa đơn, bạn có thể chuyển hướng đến trang chi tiết hóa đơn
            return RedirectToAction("Details", "OrderDetails_64131000", new { id = newOrder.OrderId });
        }

        // GET: Orders_64131000/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // Các action khác (Create, Edit, Delete) giữ nguyên như trước
    }
}
