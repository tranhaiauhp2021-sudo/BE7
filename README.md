# MyStore — ASP.NET MVC5 / Entity Framework, Customer Site (bài #13–#18)

> **Bộ mã nguồn bổ sung để GHÉP vào project ASP.NET MVC5 Database First có sẵn, không phải Visual Studio solution hoàn chỉnh**. File `.sln`, `.csproj`, `.edmx`, `Web.config` kết nối SQL Server chưa thể tạo chính xác chỉ từ slide và SQL của giảng viên. Không nên nộp nguyên ZIP này như một ứng dụng đã build thành công.

## Đã thực hiện

- **#13**: Metadata / Partial Class cho 6 bảng `User`, `Customer`, `Category`, `Product`, `Order`, `OrderDetail`.
- **#14**: Trang Home, banner, tìm kiếm, 10 sản phẩm nổi bật và 20 sản phẩm ít lượt mua nhất, phân trang 6/trang; dùng Partial Views.
- **#15**: Chi tiết sản phẩm, chọn số lượng, 8 sản phẩm liên quan, 8 sản phẩm Top Deals, phân trang.
- **#16**: Giỏ hàng Session (thêm / cập nhật / xóa / chọn sản phẩm tính tổng / danh mục / gợi ý tương tự).
- **#17**: Đăng ký tài khoản với `RegisterVM`, lưu đồng thời `User` và `Customer`.
- **#18**: Đăng nhập / Đăng xuất với `LoginVM`, `Session`, `FormsAuthentication` và menu theo trạng thái đăng nhập.

Phần #17–#18 được ghi rõ từng file và từng bước tại **[docs/BAI17_18_DANG_KY_DANG_NHAP.md](docs/BAI17_18_DANG_KY_DANG_NHAP.md)**. Phần giỏ hàng tại **[docs/BAI16_GIO_HANG.md](docs/BAI16_GIO_HANG.md)**.

## Yêu cầu trước khi chạy

- Windows, Visual Studio (ASP.NET and web development), **.NET Framework 4.7.2 hoặc 4.8**, SQL Server / SSMS.
- ASP.NET MVC5, Entity Framework 6 Database First và PagedList.Mvc (cài qua NuGet trong project Visual Studio).
- Tạo database `MyStore` từ `SQL/database2024_original.txt`; file gốc chỉ có `CREATE TABLE`, phải tạo DB trước. Model và DbContext tên `MyStoreEntities` như slide.
- Thêm phần Forms Authentication của `docs/WEB_CONFIG_FORMS_AUTH.xml` vào `Web.config` của project **bên trong `<system.web>`**. Giá trị `timeout="60"` là **60 phút**.
- Copy thư mục `Controllers`, `Models/Metadata`, `Models/ViewModel`, `Services`, `Views/Home`, `Views/Cart`, `Views/Account`, `Views/Shared`, `Content` vào project gốc; sao lưu các file trùng tên. Nếu namespace thật khác `MyStore`, đổi lại cho đúng.
- Chạy thử các luồng Home → ProductDetail → Cart → Register → Login → Logout trước khi nộp.

## Những giới hạn cần biết

- Mã nguồn hiện tại chưa có file `.sln`, `.csproj` hay `.edmx` thực tế, chưa compile được trong Visual Studio tại đây: phải ghép vào solution của bạn và chạy test trên Windows.
- Các chức năng **Checkout/đặt hàng**, Profile/Edit, Password Change, Admin CRUD, lịch sử đơn hàng **chưa được triển khai**.
- Tài khoản với `Password` lưu dạng **plaintext** theo video sẽ KHÔNG đăng nhập được với `PasswordHasher` phiên bản này; dùng account đăng ký mới. SQL có `Password NCHAR(50)` nên dùng định dạng PBKDF2 47 ký tự; muốn sản phẩm thực tế phải đổi schema và dùng ASP.NET Identity.
- Giá trị `UserRole` phải là `"C"` hoặc `"A"`, vì `dbo.User.UserRole` là `NCHAR(1)`.
- Kết nối database `Web.config` chứa dữ liệu riêng tư; không commit mật khẩu vào GitHub.

## Cách nộp GitHub

1. Tạo/mở project Visual Studio của bạn, ghép các file ở trên, **build thành công** và kiểm tra database.
2. GitHub repository phải có **solution và project chạy được** (`.sln`, `.csproj`, `.edmx`, C#, Views, CSS, SQL mẫu, README). Không push `bin/`, `obj/`, `.vs/`, khóa / mật khẩu DB.
3. Mở terminal tại thư mục solution và dùng:

```bash
git init
git add .
git commit -m "Implement MVC5 registration login logout for customer site"
git branch -M main
git remote add origin https://github.com/USERNAME/REPOSITORY.git
git push -u origin main
```

Nếu repo đã có trước, chỉ cần `git add .`, `git commit -m "Add customer authentication"`, `git push`; không chạy lại lệnh `git remote add origin`.

## Cấu trúc chính

```text
Controllers/AccountController.cs
Controllers/CartController.cs
Controllers/HomeController.cs
Models/Metadata/*.cs
Models/ViewModel/RegisterVM.cs
Models/ViewModel/LoginVM.cs
Models/ViewModel/Cart.cs, CartItem.cs, CartService.cs, ...
Services/PasswordHasher.cs
Views/Account/Register.cshtml, Login.cshtml
Views/Home/*
Views/Cart/*
Views/Shared/_CustomerLayout.cshtml
Content/myStyle/CustomerAccount.css, CustomerHome.css, ...
SQL/database2024_original.txt
docs/BAI17_18_DANG_KY_DANG_NHAP.md
docs/WEB_CONFIG_FORMS_AUTH.xml
```
