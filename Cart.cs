using System;
using System.Collections.Generic;
using System.Linq;
using MyStore.Models;
using PagedList;

namespace MyStore.Models.ViewModel
{
    // Quản lý giỏ hàng trong Session theo mẫu bài #16.
    [Serializable]
    public class Cart
    {
        private readonly List<CartItem> items = new List<CartItem>();
        // Chỉ lưu giỏ hàng vào Session; kết quả truy vấn sản phẩm tương tự là dữ liệu View.
        [NonSerialized]
        private IPagedList<Product> similarProducts;

        public IEnumerable<CartItem> Items => items;
        public List<IGrouping<string, CartItem>> GroupedItems => items
            .GroupBy(i => string.IsNullOrWhiteSpace(i.Category) ? "Chưa phân loại" : i.Category)
            .OrderBy(g => g.Key).ToList();

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public IPagedList<Product> SimilarProducts
        {
            get { return similarProducts; }
            set { similarProducts = value; }
        }

        public int ItemCount => items.Sum(i => i.Quantity);
        public decimal TotalValue() => items.Sum(i => i.TotalPrice);
        public decimal SelectedTotal => items.Where(i => i.IsSelected).Sum(i => i.TotalPrice);
        public int SelectedCount => items.Count(i => i.IsSelected);

        public void AddItem(int productId, string productImage, string productName,
            decimal unitPrice, int quantity, int categoryId, string category)
        {
            if (quantity < 1 || quantity > 100) throw new ArgumentOutOfRangeException(nameof(quantity));
            var current = items.FirstOrDefault(i => i.ProductID == productId);
            if (current == null)
            {
                items.Add(new CartItem
                {
                    ProductID = productId,
                    ProductName = productName,
                    ProductImage = productImage,
                    UnitPrice = unitPrice,
                    Quantity = quantity,
                    CategoryID = categoryId,
                    Category = category,
                    IsSelected = true
                });
            }
            else
            {
                current.Quantity = Math.Min(100, current.Quantity + quantity);
                current.IsSelected = true;
            }
        }

        public void RemoveItem(int productId) => items.RemoveAll(i => i.ProductID == productId);
        public void Clear() => items.Clear();

        public bool UpdateQuantity(int productId, int quantity)
        {
            if (quantity < 1 || quantity > 100) return false;
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item == null) return false;
            item.Quantity = quantity;
            return true;
        }

        public void UpdateSelection(IEnumerable<int> selectedProductIds)
        {
            var ids = new HashSet<int>(selectedProductIds ?? Enumerable.Empty<int>());
            foreach (var item in items)
                item.IsSelected = ids.Contains(item.ProductID);
        }
    }
}
