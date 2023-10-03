using System.ComponentModel.DataAnnotations;

namespace SoftVersionControl.Models
{
    public class Password
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        public string Username { get; set; }

        public string Hoten { get; set; }

        public string Pwd { get; set; }

        public string Role { get; set; }
    }
}