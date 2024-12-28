using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLPhong.Models;
using System.Text.RegularExpressions;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace Home.Controllers
{
    public class UserController : Controller
    {
        private QlphongTroContext ql = new QlphongTroContext();
        // GET: Home

        public IActionResult Index()
        {
            return View();
        }
        [Route("DanhMucPhongUser")]
        public IActionResult DanhMucPhongUser(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstPhong = ql.Phongs.AsNoTracking().OrderBy(x => x.MaPhong);
            PagedList<Phong> lst = new PagedList<Phong>(lstPhong, pageNumber, pageSize);
            return View(lst);
        }
        public IActionResult DetailsRoomUser(int id)
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

        public IActionResult Search(string searchString, bool? daThue, string sortOrder)
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

            // Sắp xếp dữ liệu theo yêu cầu của người dùng
            if (!String.IsNullOrEmpty(sortOrder))
            {
                if (sortOrder == "asc")
                {
                    phongs = phongs.OrderBy(p => p.GiaThueThang);
                }
                else if (sortOrder == "desc")
                {
                    phongs = phongs.OrderByDescending(p => p.GiaThueThang);
                }
            }

            // Chuyển danh sách phòng sang IPagedList
            var pageNumber = 1; // Trang hiện tại
            var pageSize = 10;  // Số lượng mục trên mỗi trang
            var pagedPhongs = phongs.ToPagedList(pageNumber, pageSize);

            return View("DanhMucPhongUser", pagedPhongs);
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
        [HttpGet]
        public IActionResult AddKHUser()
        {
            ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
            return View();
        }
        [HttpPost]
        public IActionResult AddKHUser(KhachThue khach)
        {
            // Kiểm tra xem số điện thoại đã tồn tại trong cơ sở dữ liệu chưa
            bool sdtExists = ql.KhachThues.Any(tk => tk.DienThoai == khach.DienThoai);
            if (sdtExists)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Số điện thoại đã tồn tại.";
                return View(khach);
            }

            // Kiểm tra xem số CMND đã tồn tại trong cơ sở dữ liệu chưa
            bool cmndExists = ql.KhachThues.Any(tk => tk.Cmnd == khach.Cmnd);
            if (cmndExists)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Số CMND đã tồn tại.";
                return View(khach);
            }

            // Kiểm tra các trường thông tin bắt buộc
            if (khach.HoTenDem == null || khach.Ten == null || khach.Cmnd == null || khach.DienThoai == null || khach.NgayVaoO == null || khach.MaPhong == null)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            // Kiểm tra tính hợp lệ của số điện thoại
            if (!IsPhoneNumberValid(khach.DienThoai))
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Số điện thoại không hợp lệ. Số điện thoại phải có 10 số.";
                return View();
            }

            // Kiểm tra tính hợp lệ của số CMND
            if (!IsVietnameseIDValid(khach.Cmnd))
            {
                ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                ViewData["err"] = "Số CMND không hợp lệ. Số CMND phải có 9 hoặc 12 chữ số.";
                return View();
            }

            // Nếu dữ liệu hợp lệ, thêm khách thuê vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                ql.KhachThues.Add(khach);
                ql.SaveChanges();

                // Cập nhật trạng thái đã thuê của phòng
                Phong phong = ql.Phongs.SingleOrDefault(p => p.MaPhong == khach.MaPhong);
                if (phong != null)
                {
                    phong.DaThue = true;
                    ql.SaveChanges();
                }

                // Chuyển hướng người dùng đến danh sách khách thuê của admin
                return RedirectToAction("DanhMucPhongUser", "User", new { success = "Bạn đã thêm khách thuê trọ thành công" });
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form thêm khách thuê
            ViewBag.MaPhongList = new SelectList(ql.Phongs.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
            return View();
        }
        public IActionResult HienThi()
        {
            string s = HttpContext.Session.GetString("TaiKhoan");
            var khachThue = ql.KhachThues.SingleOrDefault(k => k.DienThoai.Contains(s));
            Phong phong = ql.Phongs.Find(khachThue.MaPhong);
            HttpContext.Session.SetString("MaKH", khachThue.MaKhachThue.ToString());
            if (phong == null)
            {
                return NotFound("Không tìm thấy phòng");
            }

            Nuoc nuoc = ql.Nuocs.SingleOrDefault(n => n.MaPhong == khachThue.MaPhong);
            Dien dien = ql.Diens.SingleOrDefault(d => d.MaPhong == khachThue.MaPhong);

            ViewBag.Nuoc = nuoc;
            ViewBag.Dien = dien;
            ViewBag.KhachThue = khachThue;

            return View(phong);
        }
        public IActionResult DonHang()
        {
            string taiKhoan = HttpContext.Session.GetString("TaiKhoan");
            var khach = ql.KhachThues.SingleOrDefault(u => u.DienThoai.Contains(taiKhoan));
            var danhSachHoaDon = ql.HoaDons
                                    .Include(hd => hd.ChiTietHoaDons)
                                        .ThenInclude(ct => ct.MaPhongNavigation) // Thực hiện include để lấy thông tin từ bảng ChiTietHoaDons và Phongs
                                    .Where(hd => hd.MaKh == khach.MaKhachThue)
                                    .ToList();
            return View(danhSachHoaDon);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public ActionResult SendComment(IFormCollection collection)
        {
            var hoTen = collection["HoTen"].ToString().Trim();
            var email = collection["Email"].ToString().Trim();
            var noiDung = collection["NoiDung"].ToString().Trim();

            if (String.IsNullOrEmpty(hoTen) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(noiDung))
            {
                ViewBag.Error = "Vui lòng điền đủ thông tin";
                return View("Contact");
            }

            QlphongTroContext ql = new QlphongTroContext();
            var danhmuckienkhach = new YKienKhach();

            danhmuckienkhach.HoTen = hoTen;
            danhmuckienkhach.Email = email;
            danhmuckienkhach.NgayGui = DateTime.Now;
            danhmuckienkhach.NoiDung = noiDung;
            danhmuckienkhach.TrangThai = false; // trạng thái = false có nghĩa là quản trị viên chưa đọc

            ql.YKienKhachs.Add(danhmuckienkhach);
            ql.SaveChanges();   

            ViewBag.Success = "Gửi Thành Công";
            return View("Contact");
        }


    }
}
