using System.ComponentModel.DataAnnotations;

namespace SoftVersionControl.Models
{
    public class SoftName
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModelId { get; set; } // Khóa ngoại đến bảng cha
        public Model? Model { get; set; } // Thuộc tính navigation
        public ICollection<Software>? Softwares { get; set; } // Thuộc tính navigation
    }
}
