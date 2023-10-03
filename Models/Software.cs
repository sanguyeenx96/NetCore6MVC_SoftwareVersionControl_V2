using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SoftVersionControl.Models
{
    public class Software
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải nhập version của phần mềm")]
        [Display(Name = "Số version")]
        public string Version { get; set; }

        [Display(Name = "Điểm thay đổi")]
        public string? DiemThayDoi { get; set; }

        [Display(Name = "Ngày áp dụng")]
        public DateTime? NgayApDung { get; set; }

        [Required(ErrorMessage = "Phải nhập tên người cài đặt")]
        [Display(Name = "Người cài đặt")]
        public string? NguoiCaiDat { get; set; }

        [Display(Name = "Ngày cài đặt")]
        public DateTime? NgayCaiDat { get; set; }

        [Display(Name = "Số lượng Jig cài đặt")]
        public int? SoLuongJigCaiDat { get; set; }

        [Required(ErrorMessage = "Phải nhập tên người xác nhận")]
        [Display(Name = "Người xác nhận")]
        public string? NguoiXacNhan { get; set; }

        [Required(ErrorMessage = "Phải nhập trạng thái")]
        [Display(Name = "Trạng thái áp dụng")]
        public bool TrangThaiApDung { get; set; }

        [Display(Name = "Đường dẫn tới File phần mềm")]
        public string Path { get; set; } // Đường dẫn đến tệp

        [Display(Name = "Tên File phần mềm")]
        public string FileName { get; set; } // Tên tệp

        [Display(Name = "Ngày giờ tạo")]
        public DateTime TimeCreated { get; set; }

        public int SoftNameId { get; set; } // Khóa ngoại đến bảng cha
        public SoftName? SoftName { get; set; } // Thuộc tính navigation
    }
}
