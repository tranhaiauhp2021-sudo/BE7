using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MyStore.Models;
using MyStore.Models.ViewModel;
using MyStore.Services;

namespace MyStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyStoreEntities db = new MyStoreEntities();

        // GET: /Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View(new RegisterVM());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");
            if (model == null) return new HttpStatusCodeResult(400);
            if (!ModelState.IsValid) return View(model);

            model.Username = (model.Username ?? "").Trim();
            model.CustomerName = (model.CustomerName ?? "").Trim();
            model.CustomerPhone = (model.CustomerPhone ?? "").Trim();
            model.CustomerEmail = (model.CustomerEmail ?? "").Trim();
            model.CustomerAddress = (model.CustomerAddress ?? "").Trim();
            if (model.Username.Length < 3)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập phải có ít nhất 3 ký tự.");
                return View(model);
            }

            if (db.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại.");
                return View(model);
            }

            // UserRole: C = Customer; A = Admin (NCHAR(1) trong SQL).
            var account = new User
            {
                Username = model.Username,
                Password = PasswordHasher.Hash(model.Password),
                UserRole = "C"
            };
            var customer = new Customer
            {
                Username = model.Username,
                CustomerName = model.CustomerName,
                CustomerPhone = model.CustomerPhone,
                CustomerEmail = model.CustomerEmail,
                CustomerAddress = model.CustomerAddress
            };

            db.Users.Add(account);
            db.Customers.Add(customer);
            try
            {
                // EF SaveChanges lưu các thay đổi trong một transaction.
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // Cũng xử lý trường hợp có người tạo trùng username ngay lúc này.
                ModelState.AddModelError("", "Không thể tạo tài khoản. Hãy kiểm tra lại tên đăng nhập hoặc dữ liệu nhập.");
                return View(model);
            }

            TempData["Success"] = "Đăng ký thành công! Hãy đăng nhập để tiếp tục.";
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated) return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginVM());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model, string returnUrl)
        {
            if (model == null) return new HttpStatusCodeResult(400);
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            model.Username = (model.Username ?? "").Trim();
            // Mật khẩu KHÔNG Trim(): khoảng trắng có thể là một phần mật khẩu.
            var user = db.Users.SingleOrDefault(u => u.Username == model.Username);
            bool isCustomer = user != null && string.Equals(
                (user.UserRole ?? "").Trim(), "C", StringComparison.OrdinalIgnoreCase);
            if (isCustomer && PasswordHasher.Verify(model.Password, user.Password))
            {
                Session["Username"] = user.Username;
                Session["UserRole"] = "C";
                FormsAuthentication.SetAuthCookie(user.Username, false);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // Đăng xuất phải dùng POST + AntiForgeryToken, tránh CSRF bằng link GET.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
