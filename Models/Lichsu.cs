using System.ComponentModel.DataAnnotations;

namespace SoftVersionControl.Models
{
    public class Lichsu
    {
        [Key]
        public int Id { get; set; }
        public string Ten { get; set; }
        public string Hanhdong { get; set; }
        public DateTime Thoigian { get; set; }
        public string Type { get; set; }
    }
}
