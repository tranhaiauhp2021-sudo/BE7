using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User { }

    public sealed class UserMetadata
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(255)]
        [DisplayName("Tên đăng nhập")]
        public string Username { get; set; }

        // NOTE: CSDL mẫu của giảng viên lưu mật khẩu dạng NCHAR(50).
        // Ứng dụng thực tế cần lưu password hash + salt thay vì mật khẩu thuần.
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }

        [Required]
        [StringLength(1)]
        [DisplayName("Vai trò: A hoặc C")]
        public string UserRole { get; set; }
    }
}
