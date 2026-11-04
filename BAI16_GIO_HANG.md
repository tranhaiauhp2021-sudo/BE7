# Bài thực hành #16 — Customer Site / Giỏ hàng

**Tài liệu đối chiếu:** slide 17–26, `Web BE Development_p3 Customer Site`, và video `BE2024 #16 Customer Site - Sử dụng ViewModel, session và PartialView để thiết kế trang Giỏ hàng` (https://youtu.be/a1r4uBYsKbg).

## 1. Phân tích trước khi code

**Model / ViewModel:**
- `Models/ViewModel/CartItem.cs`: thông tin một sản phẩm, `Category`, cờ `IsSelected`, `TotalPrice`.
- `Models/ViewModel/Cart.cs`: danh sách trong giỏ, `GroupedItems`, tổng tiền, `SimilarProducts`, cập nhật/xóa.
- `Models/ViewModel/CartService.cs`: quản lý đối tượng `Cart` trong ASP.NET `Session`.

**Controller:**
- `CartController.Index`: hiển thị giỏ theo danh mục và tìm sản phẩm tương tự từ DB.
- `CartController.AddItem`: thêm sản phẩm (POST).
- `CartController.UpdateQuantity`: cập nhật số lượng (POST).
- `CartController.RemoveItem`: xóa một sản phẩm (POST).
- `CartController.ClearCart`: xóa hết giỏ (POST).
- `CartController.UpdateSelection`: lưu các checkbox chọn mua và tính tổng tiền đã chọn (POST, bổ sung để thực hiện bảng mô tả giao diện slide 18/22).
- `HomeController.ProductDetail` (POST): dùng `CartService` chung để thêm giỏ.

**View:**
- `Views/Cart/Index.cshtml`: nhóm danh mục, ảnh, tên, số lượng, giá, lựa chọn, tổng tiền và nút tiếp tục mua.
- `Views/Cart/_PVSimilarProduct.cshtml`: danh sách sản phẩm cùng loại, phân trang.
- `Content/myStyle/CustomerCart.css`: giao diện responsive.

## 2. Luồng hoạt động

1. Vào `/Home/Index` → chọn sản phẩm → `/Home/ProductDetail/{id}`.
2. Chọn số lượng 1–100, **Thêm vào giỏ hàng** → POST Home/ProductDetail → `CartService.GetCart()` → `Cart.AddItem()`.
3. Chuyển `/Cart/Index`; giỏ được lưu trong `Session["MyStoreCart"]`.
4. Cập nhật số lượng hoặc xóa 1 sản phẩm bằng POST có AntiForgeryToken; server trả về giỏ đã cập nhật.
5. Bỏ dấu chọn sản phẩm và nhấn **Cập nhật lựa chọn**; tổng tiền bên phải chỉ tính mặt hàng còn được chọn.
6. **Sản phẩm tương tự** lấy từ những danh mục của giỏ, không bao gồm sản phẩm đang trong giỏ, 6 món/trang.
7. Xóa toàn bộ giỏ → `Session.Remove("MyStoreCart")` → trang giỏ rỗng.

## 3. Kiểm thử theo checklist

- [ ] Build Visual Studio không có lỗi namespace / `MyStoreEntities` / `PagedList`.
- [ ] Add sản phẩm từ trang Detail; giỏ hiển thị ảnh, tên, đơn giá, số lượng, thành tiền.
- [ ] Add lại cùng sản phẩm → tăng số lượng nhưng không tạo dòng trùng; giới hạn tối đa 100.
- [ ] Chọn 2 sản phẩm thuộc 2 danh mục → giỏ tự nhóm danh mục.
- [ ] Sửa số lượng → thành tiền và tổng tiền được cập nhật.
- [ ] Bỏ tick 1 sản phẩm → **Cập nhật lựa chọn** → `SelectedTotal` giảm; tổng giỏ không đổi.
- [ ] Bỏ tick tất cả → tổng tiền đã chọn = 0.
- [ ] Xóa 1 sản phẩm; xóa tất cả; nhấn **Tiếp tục mua hàng** về Home.
- [ ] Sản phẩm tương tự cùng danh mục không có sản phẩm đã trong giỏ; phân trang hoạt động khi đủ dữ liệu.
- [ ] Refresh trang và đi lại giữa Home / Cart → giỏ vẫn giữ trong Session hiện tại.
- [ ] Trên điện thoại, không vỡ giao diện.

## 4. Trước khi đẩy GitHub

Tài liệu này là **mã nguồn bổ sung**, không phải file solution `.sln` độc lập. Hãy ghép vào project MVC5 đã kết nối SQL Server theo README gốc, build và test trên Windows. Không commit `Web.config` có password, thư mục `.vs/`, `bin/`, `obj/`. Dùng Git trong thư mục gốc chứa `.sln`:

```bash
git add .
git commit -m "Implement BE2024 #16 customer shopping cart"
git push
```

**Không phải phần #16:** `OrderController.Checkout`, thanh toán, xác nhận đơn hàng và đăng nhập. Nút thanh toán đang tắt có chủ đích.
