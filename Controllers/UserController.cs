using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftVersionControl.Models;

namespace SoftVersionControl.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly AppDbContext _context;
        public UserController(ILogger<UserController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LuuLichSu(string type, string hanhdong)
        {
            ISession session = HttpContext.Session;
            string role = session.GetString("userrole").ToString();
            Lichsu lichsu = new Lichsu();
            if (role == "admin")
            {
                lichsu.Ten = session.GetString("username").ToString() + " " + "(Admin)";
            }
            else
            {
                lichsu.Ten = session.GetString("username").ToString();
            }
            lichsu.Hanhdong = hanhdong;
            lichsu.Thoigian = DateTime.Now;
            lichsu.Type = type;
            _context.Lichsus.Add(lichsu);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectListNguoiXacNhan()
        {
            List<Password> nguoixacnhan = await _context.Passwords.Where(x => x.Role == "admin").ToListAsync();
            return Json(nguoixacnhan);
        }

        [HttpGet]
        public async Task<IActionResult> GetDanhsachtaikhoan()
        {
            var list = await _context.Passwords.ToListAsync();
            return PartialView("_Quanlytaikhoan", list);
        }

        [HttpPost]
        public async Task<IActionResult> ThemTaiKhoanMoi(string usn, string pwd, string hoten)
        {
            var checktrungusn = _context.Passwords.Where(x => x.Username == usn).Count();
            if (checktrungusn > 0)
            {
                return Json(new { success = false });
            }
            try
            {
                Password user = new Password();
                user.Hoten = hoten;
                user.Username = usn;
                user.Pwd = pwd;
                user.Role = "user";
                user.Type = "login";
                _context.Add(user);
                await _context.SaveChangesAsync();
                await LuuLichSu("Create", "Tạo tài khoản mới cho : " + hoten);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> XoaTaiKhoan(int id)
        {
            var user = await _context.Passwords.FindAsync(id);
            if (user != null)
            {
                _context.Passwords.Remove(user);
                await _context.SaveChangesAsync();
                await LuuLichSu("Delete", "Xoá tài khoản : " + user.Username + " , " + user.Hoten);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }     
        }

        [HttpPost]
        public async Task<IActionResult> Sua(int id, string usn, string pwd, string hoten)
        {
            var checktrungusn = _context.Passwords.Where(x => x.Username == usn).Count();
            if (checktrungusn > 1)
            {
                return Json(new { success = false });
            }
            var user = await _context.Passwords.FindAsync(id);
            if (user != null)
            {
                user.Username = usn;
                user.Pwd = pwd;
                user.Hoten = hoten;
                await _context.SaveChangesAsync();
                await LuuLichSu("Edit", "Sửa thông tin tài khoản : " + user.Username + " , " + user.Hoten);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Phanquyen(int id,string role)
        {
            var user = await _context.Passwords.FindAsync(id);
            if (user != null)
            {
                user.Role = role;
                await _context.SaveChangesAsync();
                await LuuLichSu("Edit", "Sửa phân quyền tài khoản : " + user.Username + " , " + user.Hoten + " , Quyền: " + role);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Doimatkhau(int id, string pwd)
        {
            var user = await _context.Passwords.FindAsync(id);
            if (user != null)
            {
                user.Pwd = pwd;
                await _context.SaveChangesAsync();
                await LuuLichSu("Edit", "Đổi mật khẩu tài khoản : " + user.Username + " , " + user.Hoten);

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

    }
}
