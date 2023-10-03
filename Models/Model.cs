using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SoftVersionControl.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Model tổng")]
        public int? ParentModelId { get; set; }

        [Required(ErrorMessage = "Phải có tên Model")]
        [Display(Name = "Tên Model")]
        public string Name { get; set; }

        public ICollection<Model>? ChildrenModel { get; set; }

        [ForeignKey("ParentModelId")]
        [Display(Name = "Model cha")]
        public Model? ParentModel { set; get; }

        public ICollection<SoftName>? SoftNames { get; set; } // Thuộc tính navigation
    }
}
