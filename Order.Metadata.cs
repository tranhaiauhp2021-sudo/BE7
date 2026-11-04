using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order { }

    public sealed class OrderMetadata
    {
        [DisplayName("Mã đơn hàng")]
        public int OrderID { get; set; }

        [Required]
        [DisplayName("Khách hàng")]
        public int CustomerID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Ngày đặt hàng")]
        public DateTime OrderDate { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999")]
        [DisplayFormat(DataFormatString = "{0:N0} đ")]
        [DisplayName("Tổng giá trị")]
        public decimal TotalAmount { get; set; }

        [DisplayName("Trạng thái thanh toán")]
        public string PaymentStatus { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [DisplayName("Địa chỉ giao hàng")]
        public string AddressDelivery { get; set; }
    }
}
