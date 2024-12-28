using System;
using System.Collections.Generic;

namespace QLPhong.Models;

public partial class ChiTietHoaDon
{
    public int MaChiTiet { get; set; }

    public int? MaHoaDon { get; set; }

    public int? MaPhong { get; set; }

    public decimal TienPhong { get; set; }

    public decimal TienDien { get; set; }

    public decimal TienNuoc { get; set; }

    public virtual HoaDon? MaHoaDonNavigation { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }
}
