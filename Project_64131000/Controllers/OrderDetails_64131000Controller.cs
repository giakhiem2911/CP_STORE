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
    public class OrderDetails_64131000Controller : Controller
    {
        private Project_64131000Entities db = new Project_64131000Entities();

        // GET: OrderDetails_64131000
        public ActionResult Index()
        {
            var orderDetails = db.OrderDetails.Include(o => o.Orders).Include(o => o.Products);
            return View(orderDetails.ToList());
        }

        // GET: OrderDetails_64131000/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // GET: OrderDetails_64131000/Create
        public ActionResult Create()
        {
            var orders = db.Orders.ToList();
            var products = db.Products.ToList();

            if (!orders.Any() || !products.Any())
            {
                ViewBag.ErrorMessage = "Không có đơn hàng hoặc sản phẩm nào để chọn.";
                return View();
            }

            ViewBag.OrderId = new SelectList(orders, "OrderId", "OrderStatus");
            ViewBag.ProductId = new SelectList(products, "ProductId", "ProductName");
            return View();
        }



        // POST: OrderDetails_64131000/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetailsId,OrderId,ProductId,Quantity,Price")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderStatus", orderDetails.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderDetails.ProductId);
            return View(orderDetails);
        }

        // GET: OrderDetails_64131000/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderStatus", orderDetails.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderDetails.ProductId);
            return View(orderDetails);
        }

        // POST: OrderDetails_64131000/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetailsId,OrderId,ProductId,Quantity,Price")] OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "OrderStatus", orderDetails.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderDetails.ProductId);
            return View(orderDetails);
        }

        // GET: OrderDetails_64131000/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        // POST: OrderDetails_64131000/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetails);
            db.SaveChanges();
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
