using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Project_64131000.Models;

namespace Project_64131000.Controllers
{
    public class Manage_64131000Controller : Controller
    {
        private Project_64131000Entities db = new Project_64131000Entities();

        // GET: Manage/Users
        public ActionResult Users()
        {
            var users = db.Users.ToList();
            return View(users); // Trả về danh sách người dùng
        }

        // GET: Manage/Users/Create
        public ActionResult Create()
        {
            return View(); // Trả về form tạo người dùng mới
        }

        // POST: Manage/Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Users"); // Chuyển hướng về danh sách người dùng
            }
            return View(user); // Trả về view cùng với mô hình để hiển thị lỗi nếu có
        }

        // GET: Manage/Users/Edit/5
        public ActionResult Edit(int id)
        {
            Users user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user); // Trả về form chỉnh sửa người dùng
        }

        // POST: Manage/Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Users"); // Chuyển hướng về danh sách người dùng
            }
            return View(user); // Trả về view cùng với mô hình để hiển thị lỗi nếu có
        }

        // GET: Manage/Users/Delete/5
        public ActionResult Delete(int id)
        {
            Users user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user); // Trả về form xác nhận xóa người dùng
        }

        // POST: Manage/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Users"); // Chuyển hướng về danh sách người dùng
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
