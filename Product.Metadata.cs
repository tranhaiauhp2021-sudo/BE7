using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product { }

    public sealed class ProductMetadata
    {
        [DisplayName("Mã sản phẩm")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [DisplayName("Danh mục")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả sản phẩm")]
        [DisplayName("Mô tả")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        [Range(typeof(decimal), "0", "9999999999999999", ErrorMessage = "Giá sản phẩm phải từ 0 trở lên")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        [DisplayName("Giá bán")]
        public decimal ProductPrice { get; set; }

        [DisplayName("Ảnh sản phẩm")]
        public string ProductImage { get; set; }
    }
}
