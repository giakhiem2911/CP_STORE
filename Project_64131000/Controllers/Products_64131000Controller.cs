using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_64131000.Models;

namespace Project_64131000.Controllers
{
    public class Products_64131000Controller : Controller
    {
        private Project_64131000Entities db = new Project_64131000Entities();
        string LayMaSP()
        {
            var maMax = db.Products.ToList().Select(n => n.ProductId).DefaultIfEmpty("SP0000").Max();
            int maSP = int.Parse(maMax.Substring(2)) + 1;
            string SP = String.Concat("000", maSP.ToString());
            return "SP" + SP.Substring(SP.Length - 4); // Đảm bảo chuỗi 4 ký tự
        }
        public ActionResult Search()
        {
            var products = db.Products.Include(n => n.Categories);
            return View(products.ToList());
        }
        [HttpPost]
        public ActionResult Search(string maSP)
        {
            var products = db.Products.Where(abc => abc.ProductId == maSP);
            return View(products.ToList());
        }
        [HttpGet]
        public ActionResult SearchProduct(string tenSP = "", string tenH = "", string tenDM = "", string giaMin = "", string giaMax = "")
        {
            // Khởi tạo các biến min và max
            decimal? min = string.IsNullOrEmpty(giaMin) ? (decimal?)null : Convert.ToDecimal(giaMin);
            decimal? max = string.IsNullOrEmpty(giaMax) ? (decimal?)null : Convert.ToDecimal(giaMax);

            // Kiểm tra và chuyển đổi giá tối thiểu
            if (!string.IsNullOrEmpty(giaMin) && decimal.TryParse(giaMin, out decimal parsedMin))
            {
                min = parsedMin;
            }

            // Kiểm tra và chuyển đổi giá tối đa
            if (!string.IsNullOrEmpty(giaMax) && decimal.TryParse(giaMax, out decimal parsedMax))
            {
                max = parsedMax;
            }

            // Lưu các giá trị tìm kiếm vào ViewBag để giữ trạng thái
            ViewBag.tenSP = tenSP;
            ViewBag.tenH = tenH;
            ViewBag.tenDM = tenDM;
            ViewBag.giaMin = giaMin;
            ViewBag.giaMax = giaMax;

            // Gọi stored procedure để tìm kiếm sản phẩm
            // Gọi stored procedure để tìm kiếm sản phẩm
            var productsQuery = db.Products.SqlQuery("EXEC SearchProducts @ProductName, @BrandName, @CategoryName, @MinPrice, @MaxPrice",
                new SqlParameter("@ProductName", (object)tenSP ?? DBNull.Value),
                new SqlParameter("@BrandName", (object)tenH ?? DBNull.Value),
                new SqlParameter("@CategoryName", (object)tenDM ?? DBNull.Value),
                new SqlParameter("@MinPrice", (object)min ?? DBNull.Value),
                new SqlParameter("@MaxPrice", (object)max ?? DBNull.Value));

            // Chuyển đổi DbSqlQuery<Products> thành List<Products>
            var products = productsQuery.ToList();

            // Thông báo nếu không có sản phẩm nào được tìm thấy
            if (!products.Any())
            {
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            }
            else
            {
                // Truyền danh sách sản phẩm vào view
                return View(products);
            }



            // Thông báo nếu không có sản phẩm nào được tìm thấy
            if (!products.Any())
            {
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            }

            return View(products);
        }

        // GET: Products_64131000
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brands).Include(p => p.Categories);
            return View(products.ToList());
        }

        // GET: Products_64131000/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products_64131000/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName");
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Products_64131000/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductDescription,Price,Unit,ProductImage,CategoryId,BrandId")] Products products)
        {
            //System.Web.HttpPostedFileBase;
            var imgSP = Request.Files["ProductImages"];
            //Lấy thông tin từ input type=file có tên ProductImages
            string postedFileName = System.IO.Path.GetFileName(imgSP.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/" + postedFileName);
            imgSP.SaveAs(path);
            if (ModelState.IsValid)
            {
                products.ProductId = LayMaSP();
                products.ProductImage = postedFileName;
                products.CreateDate = DateTime.Now;
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", products.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", products.CategoryId);
            return View(products);
        }

        // GET: Products_64131000/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", products.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", products.CategoryId);
            return View(products);
        }

        // POST: Products_64131000/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(String id, [Bind(Include = "ProductId,ProductName,ProductDescription,Price,Unit,ProductImage,CategoryId,BrandId")] Products products)
        {
            var imgSP = Request.Files["ProductImages"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgSP.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/" + postedFileName);
                imgSP.SaveAs(path);
            }
            catch
            { }
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandId = new SelectList(db.Brands, "BrandId", "BrandName", products.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", products.CategoryId);
            return View(products);
        }

        // GET: Products_64131000/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products_64131000/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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
