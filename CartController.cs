using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MyStore.Models;
using MyStore.Models.ViewModel;
using PagedList;

namespace MyStore.Controllers
{
    public class CartController : Controller
    {
        private readonly MyStoreEntities db = new MyStoreEntities();

        private CartService GetCartService() => new CartService(Session);

        // GET /Cart/Index -- nhóm theo Category và hiển thị sản phẩm tương tự.
        [HttpGet]
        public ActionResult Index(int? page)
        {
            var cart = GetCartService().GetCart();
            var ids = cart.Items.Select(i => i.ProductID).ToList();
            var categoryIds = cart.Items.Select(i => i.CategoryID).Distinct().ToList();
            int currentPage = Math.Max(1, page ?? 1);

            // Truy vấn ở CSDL thay vì lấy toàn bộ Products về RAM.
            var similar = db.Products.Include(p => p.Category)
                .Where(p => categoryIds.Contains(p.CategoryID) && !ids.Contains(p.ProductID))
                .OrderBy(p => p.ProductID);

            cart.PageNumber = currentPage;
            cart.SimilarProducts = similar.ToPagedList(currentPage, cart.PageSize);
            return View(cart);
        }

        // POST /Cart/AddItem -- không dùng GET để thay đổi giỏ hàng.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(int productId, int quantity = 1)
        {
            if (quantity < 1 || quantity > 100)
            {
                TempData["Error"] = "Số lượng phải từ 1 đến 100.";
                return RedirectToAction("Index");
            }
            var product = db.Products.Include(p => p.Category)
                .FirstOrDefault(p => p.ProductID == productId);
            if (product == null) return HttpNotFound();

            GetCartService().GetCart().AddItem(
                product.ProductID, product.ProductImage, product.ProductName,
                product.ProductPrice, quantity, product.CategoryID,
                product.Category == null ? "Chưa phân loại" : product.Category.CategoryName);
            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveItem(int productId)
        {
            GetCartService().GetCart().RemoveItem(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            if (!GetCartService().GetCart().UpdateQuantity(productId, quantity))
                TempData["Error"] = "Không thể cập nhật. Số lượng cần từ 1 đến 100.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSelection(int[] selectedProductIds)
        {
            GetCartService().GetCart().UpdateSelection(selectedProductIds);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
