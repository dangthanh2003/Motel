using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLPhong.Models;
using System.Text.RegularExpressions;
using X.PagedList;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Security.Cryptography;
using System.Text;



namespace QLPhong.Controllers
{

    public class HomeController : Controller
    {
        QlphongTroContext ql = new QlphongTroContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Chuyển đổi input string thành một mảng byte và tính toán băm.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Tạo một StringBuilder để chứa mảng byte và tạo một chuỗi mã hóa.
                StringBuilder sBuilder = new StringBuilder();

                // Lặp qua mỗi byte của mảng băm và định dạng nó thành một chuỗi hex.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Trả về chuỗi mã hóa.
                return sBuilder.ToString();
            }
        }
        public IActionResult Login(string success)
        {
            //Câu lệnh truyền vào view khi CRUD thành công
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string sdt, string password)
        {
            // Mã hóa mật khẩu nhập vào
            string hashedPassword = GetMd5Hash(password);

            // Kiểm tra tài khoản có trong database không
            var user = ql.TaiKhoans.FirstOrDefault(u => u.SoDienThoai == sdt && u.MatKhau == hashedPassword);

            if (user != null)
            {
                if (user.ChucVu.Contains("Quản lý"))
                {
                    HttpContext.Session.SetString("TaiKhoan", user.SoDienThoai); // Lưu tên tài khoản vào Session
                    return RedirectToAction("DanhMucPhong");
                }
                else
                {
                    HttpContext.Session.SetString("TaiKhoan", user.SoDienThoai); // Lưu tên tài khoản vào Session
                    return RedirectToAction("Index", "User");
                }
            }
            // Thông báo không có tài khoản trên hoặc mật khẩu không đúng
            ViewData["err"] = "Tài khoản hoặc mật khẩu không đúng";
            return View();
        }


        [Route("DanhMucPhong")]
        public IActionResult DanhMucPhong(int? page, string error, string success)
        {
            // Kiểm tra nếu có thông báo thành công
            if (!string.IsNullOrEmpty(error))
            {
                ViewData["err"] = error;
            }
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }


            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstPhong = ql.Phongs.AsNoTracking().OrderBy(x => x.MaPhong);
            PagedList<Phong> lst = new PagedList<Phong>(lstPhong, pageNumber, pageSize);
            return View(lst);
        }
        [Route("DanhMucYKien")]
        public IActionResult DanhMucYKien(int? page, string success)
        {
            // Kiểm tra nếu có thông báo thành công
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }

            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstYKienKhach = ql.YKienKhachs.AsNoTracking().OrderBy(x => x.MaYKien);
            PagedList<YKienKhach> lst = new PagedList<YKienKhach>(lstYKienKhach, pageNumber, pageSize);
            return View(lst);
        }
        public IActionResult YKienChiTiet(int id)
        {
            var yKien = ql.YKienKhachs.FirstOrDefault(y => y.MaYKien == id);
            if (yKien == null)
            {
                return NotFound();
            }
            // Cập nhật trạng thái của ý kiến thành "Đã xem"
            yKien.TrangThai = true;
            ql.SaveChanges();
            return View(yKien);
        }




        [Route("DanhMucNuoc")]
        public IActionResult DanhMucNuoc(int? page, string success)
        {
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstNuoc = ql.Nuocs
                                      .Include(k => k.MaPhongNavigation)
                                      .AsNoTracking()
                                      .OrderBy(x => x.MaNuoc); ;
            PagedList<Nuoc> lst = new PagedList<Nuoc>(lstNuoc, pageNumber, pageSize);
            return View(lst);
        }
        [Route("DanhMucDien")]
        public IActionResult DanhMucDien(int? page, string success)
        {
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstDien = ql.Diens
                                     .Include(k => k.MaPhongNavigation)
                                     .AsNoTracking()
                                     .OrderBy(x => x.MaDien);
            PagedList<Dien> lst = new PagedList<Dien>(lstDien, pageNumber, pageSize);
            return View(lst);
        }
        [Route("DanhMucHoaDon")]
        public IActionResult DanhMucHoaDon(int? page, string success)
        {
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            // Thêm Include cho MaPhongNavigation và MaKhNavigation
            var lstHoaDon = ql.HoaDons
                                .Include(k => k.MaPhongNavigation)
                                .Include(k => k.MaKhNavigation)
                                .AsNoTracking()
                                .OrderBy(x => x.MaHoaDon);

            PagedList<HoaDon> lst = new PagedList<HoaDon>(lstHoaDon, pageNumber, pageSize);
            return View(lst);
        }

        [Route("DanhMucKhachThue")]
        public IActionResult DanhMucKhachThue(int? page, string success)
        {
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            // Thực hiện eager loading cho MaPhongNavigation
            var lstKhachThue = ql.KhachThues
                                    .Include(k => k.MaPhongNavigation)
                                    .AsNoTracking()
                                    .OrderBy(x => x.MaKhachThue);

            PagedList<KhachThue> lst = new PagedList<KhachThue>(lstKhachThue, pageNumber, pageSize);
            return View(lst);
        }
        [Route("DanhMucTaiKhoan")]
        public IActionResult DanhMucTaiKhoan(int? page, string success)
        {
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstTaiKhoan = ql.TaiKhoans.AsNoTracking().OrderBy(x => x.MaTaiKhoan);
            PagedList<TaiKhoan> lst = new PagedList<TaiKhoan>(lstTaiKhoan, pageNumber, pageSize);
            return View(lst);
        }

        public IActionResult Search(string searchString, bool? daThue)
        {
            // Truy vấn tất cả phòng
            var phongs = from p in ql.Phongs select p;
            // Kiểm tra xem chuỗi nhập vào có null không
            if (!String.IsNullOrEmpty(searchString) && searchString != "")
            {
                phongs = phongs.Where(p => p.SoPhong.Contains(searchString));
            }
            // Kiểm tra xem trạng thái combobox người dùng chọn
            if (daThue.HasValue)
            {
                phongs = phongs.Where(p => p.DaThue == daThue.Value);
            }
            // Chuyển danh sách phòng sang IPagedList
            var pageNumber = 1; // Trang hiện tại
            var pageSize = 10;  // Số lượng mục trên mỗi trang
            var pagedPhongs = phongs.ToPagedList(pageNumber, pageSize);
            return View("DanhMucPhong", pagedPhongs);
        }
        public IActionResult SearchDien(string searchString, int? page)
        {
            // Bao gồm MaPhongNavigation để có thể truy cập SoPhong
            var dien = ql.Diens.Include(p => p.MaPhongNavigation).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                // Truy vấn các phòng có SoPhong chứa chuỗi tìm kiếm
                dien = dien.Where(p => p.MaPhongNavigation.SoPhong.Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View("DanhMucDien", dien.OrderBy(p => p.MaPhong).ToPagedList(pageNumber, pageSize));
        }

        public IActionResult SearchNuoc(string searchString, int? page)
        {
            var nuoc = ql.Nuocs.Include(p => p.MaPhongNavigation).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                nuoc = nuoc.Where(p => p.MaPhongNavigation.SoPhong.Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View("DanhMucNuoc", nuoc.OrderBy(p => p.MaPhong).ToPagedList(pageNumber, pageSize));
        }



        public IActionResult SearchTen(string searchString, int? page)
        {
            var kh = from p in ql.KhachThues.Include(k => k.MaPhongNavigation)
                     select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                kh = kh.Where(p => p.Ten.Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View("DanhMucKhachThue", kh.OrderBy(p => p.MaKhachThue).ToPagedList(pageNumber, pageSize));
        }
        public IActionResult SearchHD(string searchString, int? selectedMonth, int? page)
        {
            var hd = from p in ql.HoaDons.Include(k => k.MaPhongNavigation).Include(k => k.MaKhNavigation) select p;

            // Tìm kiếm theo số phòng
            if (!String.IsNullOrEmpty(searchString))
            {
                hd = hd.Where(p => p.MaPhongNavigation.SoPhong.Contains(searchString));
            }

            // Tìm kiếm theo tháng
            if (selectedMonth.HasValue)
            {
                hd = hd.Where(p => p.NgayLap.Month == selectedMonth);
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View("DanhMucHoaDon", hd.OrderBy(p => p.MaHoaDon).ToPagedList(pageNumber, pageSize));
        }

        public IActionResult SearchTK(string searchString, int? page, string chucvu)
        {
            var tk = from p in ql.TaiKhoans select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                tk = tk.Where(p => p.SoDienThoai.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(chucvu))
            {
                tk = tk.Where(p => p.ChucVu.Contains(chucvu));
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View("DanhMucTaiKhoan", tk.OrderBy(p => p.MaTaiKhoan).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult AddRoom()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRoom(Phong phong, IFormFile fileUpload)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem phòng có tồn tại không
                bool roomExists = ql.Phongs.Any(p => p.SoPhong == phong.SoPhong);
                if (roomExists)
                {
                    ViewData["err"] = "Số phòng đã tồn tại.";
                    return View(phong);
                }

                // Xử lý tệp ảnh
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    // Lưu tệp ảnh và nhận đường dẫn tương đối
                    phong.ImageUrl = await SaveImage(fileUpload);
                }

                // Thêm phòng
                ql.Phongs.Add(phong);
                ql.SaveChanges();
                return RedirectToAction("DanhMucPhong", "Home", new { success = "Bạn đã thêm phòng thành công" });
            }

            return View(phong);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Anh", fileName);

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return fileName; // Trả về đường dẫn tương đối
        }

        [HttpGet]

        public IActionResult EditRoom(int id)
        {
            //Truy vấn phòng theo mã phòng
            var room = ql.Phongs.SingleOrDefault(u => u.MaPhong == id);
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoom(Phong phong, IFormFile fileUpload)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng đã chọn ảnh mới chưa
                if (fileUpload != null && fileUpload.Length > 0)
                {
                    // Lưu ảnh mới và cập nhật tên ảnh trong cơ sở dữ liệu
                    phong.ImageUrl = await SaveImage(fileUpload);
                }

                // Cập nhật thông tin phòng trong cơ sở dữ liệu
                ql.Phongs.Update(phong);
                ql.SaveChanges();

                return RedirectToAction("DanhMucPhong", "Home", new { success = "Bạn đã cập nhật thông tin phòng thành công" });
            }

            return View(phong);
        }
        public IActionResult DetailsRoom(int id)
        {
            Phong phong = ql.Phongs.Find(id);

            if (phong == null)
            {
                return NotFound("Không tìm thấy phòng");
            }


            Nuoc nuoc = ql.Nuocs.SingleOrDefault(n => n.MaPhong == id);
            Dien dien = ql.Diens.SingleOrDefault(d => d.MaPhong == id);
            List<KhachThue> khachThue = ql.KhachThues.Where(k => k.MaPhong == id).ToList();

            ViewBag.Nuoc = nuoc;
            ViewBag.Dien = dien;
            ViewBag.KhachThue = khachThue;

            return View(phong);
        }
        public ActionResult DeleteRoom(int id)
        {
            Phong roomToDelete = ql.Phongs.Find(id);

            if (roomToDelete == null)
            {
                return NotFound("Không tìm thấy phòng để xóa");
            }


            var khachThue = ql.KhachThues.FirstOrDefault(k => k.MaPhong == id);
            if (khachThue != null)
            {
                return RedirectToAction("DanhMucPhong", new { error = "Không thể xóa phòng đang được cho thuê." });
            }
            //Xóa chỉ số điện trước
            var dienRecords = ql.Diens.Where(d => d.MaPhong == id);
            ql.Diens.RemoveRange(dienRecords);
            //Xóa chỉ số nước
            var nuocRecords = ql.Nuocs.Where(n => n.MaPhong == id);
            ql.Nuocs.RemoveRange(nuocRecords);
            //Xóa chi tiết hóa đơn
            var chiTietHoaDonList = ql.ChiTietHoaDons.Where(ct => ct.MaPhong == id);
            ql.ChiTietHoaDons.RemoveRange(chiTietHoaDonList);
            var HoaDonList = ql.HoaDons.Where(ct => ct.MaPhong == id);
            ql.HoaDons.RemoveRange(HoaDonList);
            ql.Phongs.Remove(roomToDelete);
            ql.SaveChanges();
            return RedirectToAction("DanhMucPhong", new { success = "Bạn đã xóa phòng thành công" });
        }
        public IActionResult TinhTienPhong(int maPhong)
        {
            Phong phong = ql.Phongs.Find(maPhong);

            if (phong == null)
            {
                return NotFound("Không tìm thấy phòng.");
            }
            // Truy vấn điện và nước của phòng
            Dien dien = ql.Diens.FirstOrDefault(d => d.MaPhong == maPhong);
            Nuoc nuoc = ql.Nuocs.FirstOrDefault(n => n.MaPhong == maPhong);

            if (dien == null || nuoc == null)
            {
                return RedirectToAction("DanhMucPhong", new { error = "Phòng này chưa tính chỉ số nước hoặc chỉ số điện." });
            }
            var kh = ql.KhachThues.FirstOrDefault(k => k.MaPhong == maPhong);

            // Lấy múi giờ hiện tại của hệ thống
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
            // Lấy thời gian cục bộ hiện tại kèm theo thông tin múi giờ
            var localTime = DateTimeOffset.Now.ToOffset(timeZone.GetUtcOffset(DateTimeOffset.Now)).DateTime;

            // Hàm tính tiền đơn giản
            decimal tienPhong = phong.GiaThueThang;
            decimal? tienDien = (dien.ChiSoMoi - dien.ChiSoCu) * dien.GiaTien;
            decimal? tienNuoc = (nuoc.ChiSoMoi - nuoc.ChiSoCu) * nuoc.GiaTien;
            decimal? thanhTien = tienPhong + tienDien + tienNuoc;

            // Tạo lập 1 hóa đơn 
            HoaDon hoadon = new HoaDon
            {
                NgayLap = new DateOnly(localTime.Year, localTime.Month, localTime.Day), // Tạo mới một đối tượng DateOnly từ ngày, tháng và năm
                ThanhTien = thanhTien.HasValue ? thanhTien.Value : 0,
                MaPhong = maPhong,
                MaKh = kh.MaKhachThue
            };
            // Thêm hóa đơn
            ql.HoaDons.Add(hoadon);
            ql.SaveChanges();

            // Lấy hóa đơn vừa tạo
            int maHoaDon = ql.HoaDons.Max(u => u.MaHoaDon);
            // Tạo chi tiết hóa đơn từ hóa đơn vừa tạo
            ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon
            {
                MaHoaDon = maHoaDon,
                MaPhong = maPhong,
                TienPhong = tienPhong,
                TienDien = tienDien.HasValue ? tienDien.Value : 0,
                TienNuoc = tienNuoc.HasValue ? tienNuoc.Value : 0
            };

            ql.ChiTietHoaDons.Add(chiTietHoaDon);
            ql.SaveChanges();

            return RedirectToAction("HoaDon", new { maHoaDon = maHoaDon });
        }

        public IActionResult InHoaDonPDF(int maHoaDon)
        {
            HoaDon hoadon = ql.HoaDons.Include(hd => hd.ChiTietHoaDons).FirstOrDefault(hd => hd.MaHoaDon == maHoaDon);

            if (hoadon == null)
            {
                return NotFound("Không tìm thấy hóa đơn.");
            }
            Document doc = new Document();
            MemoryStream memoryStream = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();

            Paragraph content = new Paragraph();
            content.Add(new Phrase("Noi dung hoa don:\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)));

            content.Add(new Chunk($"Ngay lap: {hoadon.NgayLap}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
            content.Add(new Chunk($"Tong tien: {hoadon.ThanhTien}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));

            content.Add(new Phrase("Chi tiet hoa don:\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)));

            foreach (var chiTiet in hoadon.ChiTietHoaDons)
            {

                Phong phong = ql.Phongs.FirstOrDefault(p => p.MaPhong == chiTiet.MaPhong);
                if (phong != null)
                {
                    content.Add(new Chunk($"So phong: {phong.SoPhong}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                }
                content.Add(new Chunk($"Tien phong: {chiTiet.TienPhong}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                content.Add(new Chunk($"Tien dien: {chiTiet.TienDien}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                content.Add(new Chunk($"Tien nuoc: {chiTiet.TienNuoc}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
            }

            doc.Add(content);

            doc.Close();

            byte[] pdfBytes = memoryStream.ToArray();
            Response.ContentType = "application/pdf";
            Response.Headers["Content-Disposition"] = "attachment; filename=Hoadon.pdf";
            return File(pdfBytes, "application/pdf", "Hoadon.pdf");
        }
        [HttpGet]
        public IActionResult AddDien()
        {
            ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
            return View();
        }

        [HttpPost]
        public IActionResult AddDien(Dien dien)
        {
            if (ModelState.IsValid)
            {
                if (ql.Diens.Any(d => d.MaPhong == dien.MaPhong))
                {
                    ViewData["err"] = "Phòng này đã có chỉ số điện.";
                    ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
                    return View(dien);
                }

                ql.Diens.Add(dien);
                ql.SaveChanges();
                return RedirectToAction("DanhMucDien", new { success = "Bạn đã thêm chỉ số điện thành công" });
            }
            ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
            return View(dien);
        }
        public IActionResult EditDien(int id)
        {
            ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
            var room = ql.Diens.SingleOrDefault(u => u.MaDien == id);
            return View(room);
        }
        [HttpPost]
        public IActionResult EditDien(Dien dien)
        {
            if (ModelState.IsValid)
            {
                Dien existingRoom = ql.Diens.Find(dien.MaDien);

                if (existingRoom == null)
                {
                    return NotFound("Không tìm thấy điện để cập nhật");
                }

                if (ql.Diens.Any(d => d.MaPhong == dien.MaPhong && d.MaDien != dien.MaDien))
                {
                    ViewData["err"] = "Phòng này đã có chỉ số điện vui lòng chọn lại .";
                    ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
                    return View(dien);
                }

                ql.Entry(existingRoom).CurrentValues.SetValues(dien);
                ql.SaveChanges();
                return RedirectToAction("DanhMucDien", new { success = "Bạn đã sửa chỉ số điện thành công" });
            }
            ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
            return View(dien);
        }
        public IActionResult DeleteDien(int id)
        {
            Dien roomToDelete = ql.Diens.Find(id);

            if (roomToDelete == null)
            {
                return NotFound("Không tìm thấy điện để xóa");
            }

            ql.Diens.Remove(roomToDelete);
            ql.SaveChanges();

            return RedirectToAction("DanhMucDien", new { success = "Bạn đã xóa chỉ số điện thành công" });
        }
        [HttpGet]
        public IActionResult AddNuoc()
        {
            ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
            return View();
        }

        [HttpPost]
        public IActionResult AddNuoc(Nuoc nuoc)
        {
            if (ModelState.IsValid)
            {
                if (ql.Nuocs.Any(d => d.MaPhong == nuoc.MaPhong))
                {
                    ViewData["err"] = "Phòng này đã có chỉ số nước.";
                    ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
                    return View(nuoc);
                }

                ql.Nuocs.Add(nuoc);
                ql.SaveChanges();
                return RedirectToAction("DanhMucNuoc", new { success = "Bạn đã thêm chỉ số nước thành công" });
            }
            ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
            return View(nuoc);
        }
        public IActionResult EditNuoc(int id)
        {
            ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
            var room = ql.Nuocs.SingleOrDefault(u => u.MaNuoc == id);
            return View(room);
        }
        [HttpPost]
        public IActionResult EditNuoc(Nuoc nuoc)
        {
            if (ModelState.IsValid)
            {
                Nuoc existingRoom = ql.Nuocs.Find(nuoc.MaNuoc);

                if (existingRoom == null)
                {
                    return NotFound("Không tìm thấy nước để cập nhật");
                }

                if (ql.Nuocs.Any(d => d.MaPhong == nuoc.MaPhong && d.MaNuoc != nuoc.MaNuoc))
                {
                    ViewData["err"] = "Phòng này đã có chỉ số nước vui lòng chọn lại .";
                    ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
                    return View(nuoc);
                }

                ql.Entry(existingRoom).CurrentValues.SetValues(nuoc);
                ql.SaveChanges();

                return RedirectToAction("DanhMucNuoc", new { success = "Bạn đã sửa chỉ số điện thành công" });
            }
            ViewBag.MaPhongList = new SelectList(ql.Phongs, "MaPhong", "SoPhong");
            return View(nuoc);
        }
        public IActionResult DeleteNuoc(int id)
        {
            Nuoc roomToDelete = ql.Nuocs.Find(id);

            if (roomToDelete == null)
            {
                return NotFound("Không tìm thấy nước để xóa");
            }

            ql.Nuocs.Remove(roomToDelete);
            ql.SaveChanges();

            return RedirectToAction("DanhMucNuoc", new { success = "Bạn đã xóa chỉ số nước thành công" });
        }
        [HttpGet]
        public IActionResult AddKH()
        {
            ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
            return View();
        }
        public bool IsPhoneNumberValid(string phoneNumber)
        {
            // Số điện thoại phải có đúng 10 số
            var phoneNumberRegex = new Regex(@"^\d{10}$");
            return phoneNumberRegex.IsMatch(phoneNumber);
        }

        public bool IsVietnameseIDValid(string vietnameseID)
        {
            // Số CMND phải có 9 hoặc 12 chữ số (đúng định dạng CMND Việt Nam)
            var vietnameseIDRegex = new Regex(@"^\d{9}$|^\d{12}$");
            return vietnameseIDRegex.IsMatch(vietnameseID);
        }

        [HttpPost]
        public IActionResult AddKH(KhachThue khach)
        {
            bool sdtExists = ql.KhachThues.Any(tk => tk.DienThoai == khach.DienThoai);

            if (sdtExists)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Số điện thoại đã tồn tại.";
                return View(khach);
            }
            bool cmndExists = ql.KhachThues.Any(tk => tk.Cmnd == khach.Cmnd);
            if (cmndExists)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Số CMND đã tồn tại.";
                return View(khach);
            }
            if (khach.HoTenDem == null || khach.Ten == null || khach.Cmnd == null || khach.DienThoai == null || khach.NgayVaoO == null || khach.MaPhong == null)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }
            if (!IsPhoneNumberValid(khach.DienThoai))
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Số điện thoại không hợp lệ. Số điện thoại phải có 10 số.";
                return View();
            }

            if (!IsVietnameseIDValid(khach.Cmnd))
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Số CMND không hợp lệ. Số CMND phải có 9 hoặc 12 chữ số.";
                return View();
            }
            if (ModelState.IsValid)
            {
                ql.KhachThues.Add(khach);
                ql.SaveChanges();
                Phong phong = ql.Phongs.SingleOrDefault(p => p.MaPhong == khach.MaPhong);
                if (phong != null)
                {
                    phong.DaThue = true;
                    ql.SaveChanges();
                }
                return RedirectToAction("DanhMucKhachThue", new { success = "Bạn đã thêm khách thuê trọ thành công" });
            }
            ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
            return View();
        }
        public IActionResult EditKH(int id)
        {
            ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
            var khach = ql.KhachThues.SingleOrDefault(u => u.MaKhachThue == id);
            return View(khach);
        }
        [HttpPost]
        public IActionResult EditKH(KhachThue khach, int MaPhongCu)
        {
            if (ModelState.IsValid)
            {
                KhachThue existingRoom = ql.KhachThues.Find(khach.MaKhachThue);

                if (existingRoom == null)
                {
                    return NotFound("Không tìm thấy khách hàng để cập nhật");
                }

                ql.Entry(existingRoom).CurrentValues.SetValues(khach);

                if (khach.MaPhong != MaPhongCu)
                {
                    Phong phongCu = ql.Phongs.SingleOrDefault(p => p.MaPhong == MaPhongCu);
                    Phong phongMoi = ql.Phongs.SingleOrDefault(p => p.MaPhong == khach.MaPhong);

                    if (phongMoi != null)
                    {
                        phongMoi.DaThue = true;
                    }

                    if (phongCu != null)
                    {
                        phongCu.DaThue = false;
                    }
                }

                ql.SaveChanges();

                return RedirectToAction("DanhMucKhachThue", new { success = "Bạn đã sửa thông tin khách hàng thành công" });
            }

            return View(khach);
        }


        public IActionResult DeleteKH(int id)
        {
            // Tìm khách hàng cần xóa
            KhachThue roomToDelete = ql.KhachThues.Find(id);

            if (roomToDelete == null)
            {
                return NotFound("Không tìm thấy khách hàng để xóa");
            }

            // Tìm phòng của khách hàng và cập nhật trạng thái DaThue
            Phong phong = ql.Phongs.SingleOrDefault(p => p.MaPhong == roomToDelete.MaPhong);
            if (phong != null)
            {
                phong.DaThue = false;
            }

            // Xóa tất cả các hóa đơn và chi tiết hóa đơn của khách hàng
            var dhList = ql.HoaDons.Where(u => u.MaKh == id).ToList();
            foreach (var cthd in dhList)
            {
                var cthdToDelete = ql.ChiTietHoaDons.Where(u => u.MaHoaDon == cthd.MaHoaDon).ToList();
                foreach (var ct in cthdToDelete)
                {
                    ql.ChiTietHoaDons.Remove(ct);
                }
                ql.HoaDons.Remove(cthd);
            }

            // Xóa khách hàng
            ql.KhachThues.Remove(roomToDelete);

            // Lưu các thay đổi vào cơ sở dữ liệu
            ql.SaveChanges();

            // Chuyển hướng đến action "DanhMucKhachThue" và truyền thông báo thành công
            return RedirectToAction("DanhMucKhachThue", new { success = "Bạn đã xóa khách hàng thành công" });
        }


        public IActionResult Hoadon(int maHoaDon)
        {
            HoaDon hoadon = ql.HoaDons.Find(maHoaDon);

            if (hoadon == null)
            {
                return NotFound("Không tìm thấy hóa đơn.");
            }

            hoadon.ChiTietHoaDons = ql.ChiTietHoaDons.Where(ct => ct.MaHoaDon == maHoaDon).ToList();

            return View(hoadon);
        }
        public IActionResult ChiTietHD(int id)
        {
            var cthd = ql.ChiTietHoaDons
                        .Include(ct => ct.MaPhongNavigation) // Include MaPhongNavigation
                        .SingleOrDefault(u => u.MaHoaDon == id);
            return View(cthd);
        }


        [HttpGet]
        public IActionResult CreateTK()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateTK(TaiKhoan taiKhoan, string NhapLaiMatKhau)
        {
            if (ModelState.IsValid)
            {
                bool sdtExists = ql.TaiKhoans.Any(tk => tk.SoDienThoai == taiKhoan.SoDienThoai);

                if (sdtExists)
                {
                    ViewData["err"] = "Số điện thoại đã tồn tại.";
                    return View(taiKhoan);
                }
                sdtExists = ql.KhachThues.Any(tk => tk.DienThoai == taiKhoan.SoDienThoai);
                if (!sdtExists)
                {
                    ViewData["err"] = "Số điện thoại bạn đang tạo không trùng với bất kì số điện thoại nào của khách hàng.";
                    return View(taiKhoan);
                }
                if (taiKhoan.MatKhau != NhapLaiMatKhau)
                {
                    ViewData["err"] = "Mật khẩu và nhập lại mật khẩu không khớp.";
                    return View(taiKhoan);
                }
                if (!IsPhoneNumberValid(taiKhoan.SoDienThoai))
                {
                    ViewData["err"] = "Số điện thoại không hợp lệ. Số điện thoại phải có 10 số.";
                    return View(taiKhoan);
                }

                // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                taiKhoan.MatKhau = GetMd5Hash(taiKhoan.MatKhau);

                ql.TaiKhoans.Add(taiKhoan);
                ql.SaveChanges();
                return RedirectToAction("DanhMucTaiKhoan", new { success = "Bạn đã tạo tài khoản mới thành công" });
            }

            return View(taiKhoan);
        }

        public ActionResult EditTK(int id)
        {
            TaiKhoan taiKhoan = ql.TaiKhoans.Find(id);

            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản để chỉnh sửa.");
            }

            return View(taiKhoan);
        }
        [HttpPost]
        public IActionResult EditTK(TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                bool sdtExists = ql.TaiKhoans.Any(tk => tk.SoDienThoai == taiKhoan.SoDienThoai && tk.MaTaiKhoan != taiKhoan.MaTaiKhoan);

                if (sdtExists)
                {
                    ViewData["err"] = "Số điện thoại đã tồn tại.";
                    return View(taiKhoan);
                }

                TaiKhoan existingTaiKhoan = ql.TaiKhoans.Find(taiKhoan.MaTaiKhoan);

                if (existingTaiKhoan == null)
                {
                    return NotFound("Không tìm thấy tài khoản để cập nhật.");
                }
                if (!IsPhoneNumberValid(taiKhoan.SoDienThoai))
                {
                    ViewData["err"] = "Số điện thoại không hợp lệ. Số điện thoại phải có 10 số.";
                    return View(taiKhoan);
                }
                ql.Entry(existingTaiKhoan).CurrentValues.SetValues(taiKhoan);
                ql.SaveChanges();
                return RedirectToAction("DanhMucTaiKhoan", new { success = "Bạn đã sửa tài khoản thành công" });
            }

            return View(taiKhoan);
        }
        public IActionResult Pass(int id, string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewData["err"] = error;
            }
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Pass(int id, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                return RedirectToAction("Pass", new { id = id, error = "Mật khẩu phải trùng" });
            }

            // Mã hóa mật khẩu mới
            string hashedPassword = GetMd5Hash(newPassword);

            // Lấy tài khoản từ cơ sở dữ liệu theo 'id' (sử dụng Entity Framework)
            var taiKhoan = ql.TaiKhoans.SingleOrDefault(tk => tk.MaTaiKhoan == id);

            if (taiKhoan != null)
            {
                // Cập nhật mật khẩu và trạng thái của tài khoản
                taiKhoan.MatKhau = hashedPassword; // Mật khẩu đã được mã hóa
                taiKhoan.TrangThai = true;

                // Lưu thay đổi vào cơ sở dữ liệu
                ql.SaveChanges();

                return RedirectToAction("DanhMucTaiKhoan", new { success = "Bạn đã cấp mật khẩu cho tài khoản thành công" });
            }
            else
            {
                return RedirectToAction("DanhMucTaiKhoan", new { error = "Bạn đã cấp mật khẩu cho tài khoản không thành công" });
            }
        }

        public IActionResult DeleteTK(int id)
        {
            TaiKhoan taiKhoan = ql.TaiKhoans.Find(id);

            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản để xóa.");
            }

            ql.TaiKhoans.Remove(taiKhoan);
            ql.SaveChanges();

            return RedirectToAction("DanhMucTaiKhoan", new { success = "Bạn đã xóa tài khoản thành công" });
        }
        [HttpGet]
        public ActionResult Forget(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewData["err"] = error;
            }
            return View();
        }
        //Hàm xử lý khi người dùng nhấn nút gửi
        [HttpPost]
        public IActionResult Forget(string phoneNumber, string s)
        {
            var tk = ql.TaiKhoans.SingleOrDefault(u => u.SoDienThoai == phoneNumber);
            if (tk == null)
            {
                return RedirectToAction("Forget", new { error = "Số điện thoại không tồn tại trong hệ thống" });
            }
            tk.TrangThai = false;
            ql.SaveChanges();
            return RedirectToAction("Login", new { success = "Yêu cầu thành công vui lòng liên hệ admin để cấp lại mật khẩu" });
        }
        public IActionResult DeleteTaiKhoan(int id)
        {
            TaiKhoan taiKhoan = ql.TaiKhoans.Find(id);

            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản để xóa.");
            }

            ql.TaiKhoans.Remove(taiKhoan);
            ql.SaveChanges();

            return RedirectToAction("DanhMucTaiKhoan", new { success = "Bạn đã xóa tài khoản thành công" });
        }

    }
}
