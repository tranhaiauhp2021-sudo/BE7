using System;

namespace MyStore.Models.ViewModel
{
    // Một sản phẩm được lưu trong Session (không cần thêm bảng vào database).
    [Serializable]
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsSelected { get; set; } = true;
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
