using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail { }

    public sealed class OrderDetailMetadata
    {
        [DisplayName("Mã chi tiết")]
        public int ID { get; set; }

        [Required]
        [DisplayName("Sản phẩm")]
        public int ProductID { get; set; }

        [Required]
        [DisplayName("Đơn hàng")]
        public int OrderID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        [DisplayName("Đơn giá")]
        public decimal UnitPrice { get; set; }
    }
}
