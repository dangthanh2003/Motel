﻿using System;
using System.Collections.Generic;

namespace QLPhong.Models;

public partial class TaiKhoan
{
    public int MaTaiKhoan { get; set; }

    public string SoDienThoai { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string ChucVu { get; set; } = null!;

    public bool? TrangThai { get; set; }
}
