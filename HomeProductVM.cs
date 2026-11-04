using System.Collections.Generic;
using PagedList;

namespace MyStore.Models.ViewModel
{
    public class HomeProductVM
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 6;
        public List<Product> FeaturedProducts { get; set; } = new List<Product>();
        public IPagedList<Product> NewProducts { get; set; }
    }
}
