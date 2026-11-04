using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category { }

    public sealed class CategoryMetadata
    {
        [DisplayName("Mã danh mục")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [DisplayName("Tên danh mục")]
        public string CategoryName { get; set; }
    }
}
