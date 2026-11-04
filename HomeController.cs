using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MyStore.Models;
using MyStore.Models.ViewModel;
using PagedList;

namespace MyStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyStoreEntities db = new MyStoreEntities();

        public ActionResult Index(string searchTerm, int? page)
        {
            var products = db.Products.Include(p => p.Category).AsQueryable();
            searchTerm = (searchTerm ?? "").Trim();

            if (!string.IsNullOrEmpty(searchTerm))
                products = products.Where(p => p.ProductName.Contains(searchTerm)
                        || p.ProductDescription.Contains(searchTerm)
                        || p.Category.CategoryName.Contains(searchTerm));

            int pageNumber = Math.Max(1, page ?? 1);
            var model = new HomeProductVM
            {
                SearchTerm = searchTerm,
                PageNumber = pageNumber,
                PageSize = 6,
                // Bán chạy nhất theo số dòng chi tiết đơn hàng (cách đếm theo slide).
                FeaturedProducts = products.OrderByDescending(p => p.OrderDetails.Count())
                        .ThenBy(p => p.ProductID).Take(10).ToList(),
                // Theo bài giảng: lấy 20 sản phẩm có ít lượt mua nhất.
                NewProducts = products.OrderBy(p => p.OrderDetails.Count())
                        .ThenBy(p => p.ProductID).Take(20).ToPagedList(pageNumber, 6)
            };
            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult CategoryMenu()
        {
            return PartialView("_CategoryMenu", db.Categories.OrderBy(c => c.CategoryName).ToList());
        }

        public ActionResult ProductList(int id, int? page)
        {
            var category = db.Categories.Find(id);
            if (category == null) return HttpNotFound();
            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategoryID = id;
            var products = db.Products.Where(p => p.CategoryID == id).OrderBy(p => p.ProductID);
            return View(products.ToPagedList(Math.Max(1, page ?? 1), 9));
        }

        [HttpGet]
        public ActionResult ProductDetail(int? id, int? page)
        {
            if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = db.Products.Include(p => p.Category)
                        .FirstOrDefault(p => p.ProductID == id.Value);
            if (product == null) return HttpNotFound();
            var sameCategory = db.Products
                .Where(p => p.CategoryID == product.CategoryID && p.ProductID != product.ProductID);
            var model = new ProductDetailVM
            {
                Product = product,
                Quantity = 1,
                RelatedProducts = sameCategory.OrderBy(p => p.ProductID).Take(8).ToList(),
                TopProducts = sameCategory.OrderByDescending(p => p.OrderDetails.Count())
                        .ThenBy(p => p.ProductID).Take(8).ToList()
            };
            model.PagedTopProducts = model.TopProducts.ToPagedList(Math.Max(1, page ?? 1), 3);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductDetail(int id, int quantity)
        {
            if (quantity < 1 || quantity > 100)
            {
                TempData["Error"] = "Số lượng phải từ 1 đến 100.";
                return RedirectToAction("ProductDetail", new { id });
            }
            var product = db.Products.Include(p => p.Category)
                .FirstOrDefault(p => p.ProductID == id);
            if (product == null) return HttpNotFound();

            // Dùng chung CartService với CartController, tránh lưu hai kiểu giỏ hàng.
            new CartService(Session).GetCart().AddItem(
                product.ProductID, product.ProductImage, product.ProductName,
                product.ProductPrice, quantity, product.CategoryID,
                product.Category == null ? "Chưa phân loại" : product.Category.CategoryName);
            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng.";
            return RedirectToAction("Index", "Cart");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
