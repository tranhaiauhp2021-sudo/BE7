# Bài #17 và #18 — Đăng ký, Đăng nhập, Đăng xuất

## Bám theo slide của giảng viên

- **Bài #17:** `RegisterVM` chứa Username, Password, ConfirmPassword, CustomerName, CustomerPhone, CustomerEmail, CustomerAddress và Data Annotations. `AccountController.Register` nhận form, kiểm tra trùng Username, thêm `User` và `Customer` và `SaveChanges`.
- **Bài #18:** `LoginVM` chứa Username/Password. `AccountController.Login` kiểm tra tài khoản, ghi `Session["Username"]`, `Session["UserRole"]`, gọi `FormsAuthentication.SetAuthCookie`. `Logout` đăng xuất, xóa Session; `_CustomerLayout` tự đổi menu.
- SQL mẫu: `UserRole NCHAR(1)` với **`A` = Admin** và **`C` = Customer**. Do đó dùng `C` chứ không phải `Customer` trong ví dụ ở slide.
- Cấu hình Forms Authentication trong `Web.config` (xem `WEB_CONFIG_FORMS_AUTH.xml`). `timeout` là **phút**, nên `60` là 1 giờ (khác ghi chú slide).

## Các file mới / file sửa

| File | Nội dung |
|---|---|
| `Models/ViewModel/RegisterVM.cs` | Form đăng ký + DataAnnotations |
| `Models/ViewModel/LoginVM.cs` | Form đăng nhập + DataAnnotations |
| `Services/PasswordHasher.cs` | PBKDF2-HMAC-SHA256, tương thích `NCHAR(50)` |
| `Controllers/AccountController.cs` | GET/POST Register, GET/POST Login, POST Logout |
| `Views/Account/Register.cshtml` | Form đăng ký |
| `Views/Account/Login.cshtml` | Form đăng nhập |
| `Content/myStyle/CustomerAccount.css` | Giao diện trang tài khoản |
| `Views/Shared/_CustomerLayout.cshtml` | Menu đăng nhập / đăng xuất |
| `Content/myStyle/CustomerHome.css` | Điều chỉnh menu |
| `docs/WEB_CONFIG_FORMS_AUTH.xml` | Mẫu chèn vào Web.config |

## Ghép vào project Visual Studio MVC5

1. Sao lưu project trước khi ghi đè bất kỳ file nào, đặc biệt `_CustomerLayout.cshtml`.
2. Sao chép `Controllers`, `Models/ViewModel`, `Services`, `Views/Account`, `Views/Shared`, `Content/myStyle` vào project `MyStore` hiện có. Giữ nguyên code bài #13–#16.
3. `Models/User.cs`, `Customer.cs` và `MyStoreEntities` phải được sinh qua Entity Framework **Database First** từ SQL mẫu (không nằm trong gói overlay này).
4. Trong `Web.config` đang dùng, tìm `<system.web>` và thêm đúng một thẻ `authentication` như nội dung `WEB_CONFIG_FORMS_AUTH.xml`. Giữ lại tất cả connection strings, runtime/package settings do project sinh; không thay toàn bộ bằng snippet.
5. Mở `Views/Account/Login.cshtml` và `Views/Account/Register.cshtml`, kiểm tra route có đúng `/Account/Login` và `/Account/Register`.
6. Build rồi kiểm thử các luồng trong phần dưới. Nếu project tên khác `MyStore`, đổi namespace theo project thật.

## Kiểm thử trong Visual Studio/IIS Express

1. Vào `/Account/Register`, cố tình bỏ trống dữ liệu => báo lỗi Required.
2. Nhập hai mật khẩu khác nhau => báo lỗi xác nhận.
3. Nhập thông tin hợp lệ => lưu 1 record `User` + 1 record `Customer`, cùng Username, `UserRole='C'`.
4. Đăng ký lại đúng Username => từ chối, không tạo bản ghi trùng.
5. Vào `/Account/Login`, đăng nhập sai => báo lỗi chung.
6. Đăng nhập đúng tài khoản **vừa đăng ký theo bản này** => về Home, menu hiện tên người dùng và nút Đăng xuất.
7. Đăng xuất => Forms Authentication cookie bị xóa và menu trở lại Đăng nhập / Đăng ký.
8. Thử với tài khoản có vai trò `A` (Admin) => từ chối login vào Customer Site.
9. Kiểm tra `SELECT Username, UserRole, LEN(RTRIM(Password)) FROM dbo.[User];` => chuỗi mật khẩu mã băm dài 47 ký tự, không phải mật khẩu thuần.

## Bảo mật và tương thích bài học

- Bản giảng minh họa lưu mật khẩu trực tiếp; bản bổ sung **không lưu mật khẩu thuần**, mà dùng PBKDF2. Vì SQL mẫu giới hạn `Password NCHAR(50)`, đây là định dạng 47 ký tự `P1$` + base64. Cột `NCHAR` tự thêm khoảng trắng và được xử lý khi verify.
- **Tài khoản demo cũ lưu mật khẩu thuần sẽ không đăng nhập bằng code này.** Hãy đăng ký tài khoản mới để kiểm thử. Không tự động fallback sang so sánh plaintext.
- Một hệ thống thật nên đổi DB sang `NVARCHAR(255)` và dùng ASP.NET Identity / password hasher chuẩn, thêm rate limiting / chống brute force / HTTPS & cookie secure, logging hợp lý. Bài này là ứng dụng học tập.
- Đăng xuất sử dụng POST + AntiForgeryToken để tránh thay đổi trạng thái qua GET. `returnUrl` chỉ điều hướng khi là URL nội bộ.

**Chưa làm trong bài #17–#18:** ProfileInfo, UpdateProfile, ChangePassword, Checkout, lịch sử đơn hàng và Admin Site; có liệt kê trong phần dự án tổng thể nhưng không phải phạm vi hai video mới.
