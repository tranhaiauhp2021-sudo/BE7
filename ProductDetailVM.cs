using System.Collections.Generic;
using PagedList;

namespace MyStore.Models.ViewModel
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;
        public List<Product> RelatedProducts { get; set; } = new List<Product>();
        public List<Product> TopProducts { get; set; } = new List<Product>();
        public IPagedList<Product> PagedTopProducts { get; set; }
    }
}
