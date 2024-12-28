using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLPhong.Models
{
    public partial class YKienKhach
    {
       
        public int MaYKien { get; set; }

        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? NgayGui { get; set; }
        public string NoiDung { get; set; } = null!;
        public bool? TrangThai { get; set; }

        
    }
}
