using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftVersionControl.Models;

namespace SoftVersionControl.Controllers
{
    public class HistoryController : Controller
    {
        private readonly AppDbContext _context;
        public HistoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Lichsu()
        {
            ViewBag.parentPage = "Trang chủ";
            ViewBag.currentPage = "Lịch sử hoạt động";
            var list = await _context.Lichsus.OrderBy(x => x.Thoigian).ToListAsync();
            return View(list);
        }
        
    }
}
