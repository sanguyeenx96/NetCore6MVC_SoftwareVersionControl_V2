using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftVersionControl.Models;
using System.Diagnostics;

namespace SoftVersionControl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
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
        public async Task<IActionResult> Login(string username, string pwd)
        {
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(pwd))
            {
                Password checklogin = await _context.Passwords.Where(x => (x.Type == "login" && x.Username == username && x.Pwd == pwd)).FirstOrDefaultAsync();
                if (checklogin != null)
                {
                    ISession session = HttpContext.Session;
                    session.SetString("username", checklogin.Hoten);
                    session.SetString("userrole", checklogin.Role);

                    var hoten = session.GetString("username");
                    await LuuLichSu("Login", "Đăng nhập hệ thống");
                    return RedirectToAction("index");
                }
            }
            return View();
        }


        public IActionResult Index()
        {
            ViewBag.parentPage = "Trang chủ";
            ViewBag.currentPage = "Dữ liệu";
            ViewBag.countModel = _context.Models.Count().ToString();
            ViewBag.countSoft = _context.SoftNames.Count().ToString();
            ViewBag.countUser = _context.Passwords.Count().ToString();
            var recentActivities = _context.Lichsus
               .OrderByDescending(ls => ls.Thoigian)
               .Take(5)
               .ToList();
            return View(recentActivities);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}