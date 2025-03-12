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
    public class Brands_64131000Controller : Controller
    {
        private Project_64131000Entities db = new Project_64131000Entities();
        public ActionResult ProductsInBrand(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy thương hiệu dựa trên ID
            Brands brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách sản phẩm thuộc thương hiệu
            var products = db.Products.Where(p => p.BrandId == brand.BrandId).ToList();

            ViewBag.BrandName = brand.BrandName; // Lưu tên thương hiệu để hiển thị trên view
            return View(products);
        }

        // GET: Brands_64131000
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        // GET: Brands_64131000/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brands brands = db.Brands.Find(id);
            if (brands == null)
            {
                return HttpNotFound();
            }
            return View(brands);
        }

        // GET: Brands_64131000/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands_64131000/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BrandId,BrandName,LogoUrl")] Brands brands)
        {
            //System.Web.HttpPostedFileBase;
            var imgH = Request.Files["LogoUrls"];
            //Lấy thông tin từ input type=file có tên ProductImages
            string postedFileName = System.IO.Path.GetFileName(imgH.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/" + postedFileName);
            imgH.SaveAs(path);
            if (ModelState.IsValid)
            {
                brands.LogoUrl = postedFileName;
                db.Brands.Add(brands);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brands);
        }

        // GET: Brands_64131000/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brands brands = db.Brands.Find(id);
            if (brands == null)
            {
                return HttpNotFound();
            }
            return View(brands);
        }

        // POST: Brands_64131000/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BrandId,BrandName,LogoUrl")] Brands brands)
        {
            var imgH = Request.Files["LogoUrls"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgH.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/" + postedFileName);
                imgH.SaveAs(path);
            }
            catch
            { }
            if (ModelState.IsValid)
            {
                db.Entry(brands).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brands);
        }

        // GET: Brands_64131000/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brands brands = db.Brands.Find(id);
            if (brands == null)
            {
                return HttpNotFound();
            }
            return View(brands);
        }

        // POST: Brands_64131000/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Brands brands = db.Brands.Find(id);
            db.Brands.Remove(brands);
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
