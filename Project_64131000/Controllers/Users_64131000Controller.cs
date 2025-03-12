using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_64131000.Models;

namespace Project_64131000.Controllers
{
    public class Users_64131000Controller : Controller
    {
        private Project_64131000Entities db = new Project_64131000Entities();

        public ActionResult Search()
        {
            var users = db.Users.ToList(); // Lấy tất cả người dùng
            return View(users);
        }

        // POST: Users/Search
        [HttpPost]
        public ActionResult Search(string username)
        {
            // Tìm kiếm người dùng theo tên đăng nhập
            var users = db.Users.Where(u => u.Username.Contains(username));
            return View(users.ToList());
        }

        // GET: Users_64131000
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users_64131000/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users_64131000/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users_64131000/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Username,PasswordHash,Email,Role,PhoneNumber")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Users_64131000/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users_64131000/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,PasswordHash,Email,Role,PhoneNumber")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users_64131000/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users_64131000/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Tìm user cần xóa
                    Users user = db.Users.Find(id);

                    if (user != null)
                    {
                        // Tìm customer tương ứng dựa trên thông tin liên kết
                        var customer = db.Customers.FirstOrDefault(c => c.Email == user.Email);
                        if (customer != null)
                        {
                            db.Customers.Remove(customer); // Xóa customer
                        }

                        db.Users.Remove(user); // Xóa user
                        db.SaveChanges();

                        // Commit transaction nếu mọi thứ thành công
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    transaction.Rollback();
                    ModelState.AddModelError("", $"Có lỗi xảy ra: {ex.Message}");
                }
            }

            return RedirectToAction("Index");
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
