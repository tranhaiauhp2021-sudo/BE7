using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    [MetadataType(typeof(CustomerMetadata))]
    public partial class Customer { }

    public sealed class CustomerMetadata
    {
        [DisplayName("Mã khách hàng")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [DisplayName("Tên khách hàng")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(15, ErrorMessage = "Số điện thoại không quá 15 ký tự")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [DisplayName("Số điện thoại")]
        public string CustomerPhone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [DisplayName("Email")]
        public string CustomerEmail { get; set; }

        [DisplayName("Địa chỉ")]
        public string CustomerAddress { get; set; }

        [Required]
        [DisplayName("Tên đăng nhập")]
        public string Username { get; set; }
    }
}
