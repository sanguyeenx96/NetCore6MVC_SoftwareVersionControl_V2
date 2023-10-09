using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SoftVersionControl.Models;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace SoftVersionControl.Controllers
{
    public class SoftController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public SoftController(ILogger<HomeController> logger, AppDbContext context)
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

        [HttpPost]
        public async Task<IActionResult> CreateSoftName(string name, int modelId)
        {
            var checktrung = _context.SoftNames.FirstOrDefault(x => (x.Name == name.ToUpper() && x.ModelId == modelId));
            if (checktrung != null)
            {
                return Json(new { success = false, message = "Model đã tồn tại phần mềm" });
            }
            var newSoftName = new SoftName { Name = name.ToUpper(), ModelId = modelId };
            _context.Add(newSoftName);
            await _context.SaveChangesAsync();

            string tenmodel = _context.Models.Where(x => x.Id == modelId).FirstOrDefault().Name.ToString();
            await LuuLichSu("Create", "Tạo phần mềm mới: " + name.ToUpper() + " thuộc Model: " + tenmodel);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetListSoftware(int modelId)
        {
            var getAll = _context.SoftNames
             .Where(x => x.ModelId == modelId)
             .Include(x => x.Softwares)
             .Include(x => x.Model);
            var danhSachphanmem = await getAll.ToListAsync();
            return PartialView("_SoftwareList", danhSachphanmem);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAllVersionOfSoftware(int softId)
        {
            var getAll = _context.Softwares
                .Where(x => x.SoftNameId == softId);
            // Truy xuất danh sách phiên bản từ cơ sở dữ liệu
            var danhsach = await getAll.ToListAsync();
            // Sắp xếp danh sách bằng cách sử dụng client-side
            danhsach = danhsach.OrderByDescending(x => Version.Parse(x.Version)).ToList();
            return Json(new { success = true, data = danhsach });
        }


        [HttpGet]
        public async Task<IActionResult> GetListNameSoft(int modelId)
        {
            List<SoftName> softNames = await _context.SoftNames.Where(x => x.ModelId == modelId).ToListAsync();
            return Json(softNames);
        }

        [HttpGet]
        public async Task<IActionResult> GetSoftNameFromId(int id)
        {
            var soft = await _context.SoftNames.Where(x => x.Id == id).FirstOrDefaultAsync();
            string nameSoft = soft.Name.ToString();
            return Json(new { success = true, data = nameSoft });
        }

        [HttpGet]
        public async Task<IActionResult> GetSoftLastVersionFromId(int id)
        {
            try
            {
                var listsoft = await _context.Softwares.Where(x => x.SoftNameId == id).OrderByDescending(s => s.Version).FirstOrDefaultAsync();
                string version = listsoft.Version.ToString();
                return Json(new { success = true, data = version });
            }
            catch
            {
                return Json(new { success = true, data = "" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVersionOfSoft(int id)
        {
            try
            {
                List<Version> vers = new List<Version>();
                var listver = await _context.Softwares.Where(x => x.SoftNameId == id).ToListAsync();
                foreach (var item in listver)
                {
                    string ver = item.Version;
                    vers.Add(Version.Parse(ver));
                }
                return Json(new { success = true, data = vers });
            }
            catch
            {
                return Json(new { success = true, data = "" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetSelectListVersion(int id)
        {
            List<Software> version = await _context.Softwares.Where(x => x.SoftNameId == id).ToListAsync();
            return Json(version);
        }

        [HttpPost]
        public async Task<IActionResult> EditSoftName(int id, string name)
        {
            var soft = await _context.SoftNames.Where(x => x.Id == id).Include(x => x.Model).FirstOrDefaultAsync();
            string tenmodel = soft.Model.Name.ToString();
            if (soft != null)
            {
                soft.Name = name;
                await _context.SaveChangesAsync();
                await LuuLichSu("Edit", "Sửa tên phần mềm " + name.ToUpper() + " thuộc Model: " + tenmodel);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSoftToEdit(int softId)
        {
            var soft = await _context.Softwares.Where(x => x.SoftNameId == softId).Include(x => x.SoftName).OrderByDescending(s => s.Version).FirstOrDefaultAsync();
            return PartialView("_ModalEditSoftInfo", soft);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllVerOfSoft(int softid)
        {
            try
            {
                var softName = await _context.SoftNames.Where(x => x.Id == softid).Include(x => x.Model).FirstOrDefaultAsync();
                var soft = await _context.Softwares.Where(x => x.SoftNameId == softid).Include(x => x.SoftName).ToListAsync();
                if (soft != null)
                {
                    foreach (var item in soft)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.Path);
                        FileInfo file = new FileInfo(filePath);

                        if (System.IO.File.Exists(filePath))
                        {
                            file.Delete();
                        }
                        _context.Softwares.Remove(item);
                    }
                    _context.SoftNames.Remove(softName);
                    await _context.SaveChangesAsync();
                }
                await LuuLichSu("Delete", "Xoá tất cả Version phần mềm " + softName.Name.ToUpper() + " thuộc Model: " + softName.Model.Name.ToString());
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete1VerOfSoft(int softid, int verSoftId)
        {
            try
            {
                var softName = await _context.SoftNames.Where(x => x.Id == softid).Include(x => x.Model).FirstOrDefaultAsync();
                var soft = await _context.Softwares.Where(x => x.SoftNameId == softid).Include(x => x.SoftName).ToListAsync();
                if (soft != null && soft.Count > 1)
                {
                    var verSoft = soft.Where(x => x.Id == verSoftId).FirstOrDefault();
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", verSoft.Path);
                    FileInfo file = new FileInfo(filePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        file.Delete();
                    }
                    _context.Softwares.Remove(verSoft);
                    await _context.SaveChangesAsync();                   
                    await LuuLichSu("Delete", "Xoá Version " + verSoft.Version.ToString() + " của phần mềm " + softName.Name.ToUpper() + " thuộc Model: " + softName.Model.Name.ToString());
                    await updateNextVersionAfterDelete(softid, verSoftId);
                }
                if (soft != null && soft.Count == 1)
                {
                    var verSoft = soft.Where(x => x.Id == verSoftId).FirstOrDefault();
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", verSoft.Path);
                    FileInfo file = new FileInfo(filePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        file.Delete();
                    }
                    _context.Softwares.Remove(verSoft);
                    _context.SoftNames.Remove(softName);
                    await _context.SaveChangesAsync();
                    await LuuLichSu("Delete", "Xoá Version " + verSoft.Version.ToString() + " của phần mềm " + softName.Name.ToUpper() + " thuộc Model: " + softName.Model.Name.ToString());
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public async Task<IActionResult> updateNextVersionAfterDelete(int softid, int verSoftId)
        {
            var softName = await _context.SoftNames.Where(x => x.Id == softid).Include(x => x.Model).FirstOrDefaultAsync();
            var soft = await _context.Softwares.Where(x => x.SoftNameId == softid).Include(x => x.SoftName).ToListAsync();
            var lastnextVersion = soft.OrderByDescending(s => s.Version).FirstOrDefault();
            if (lastnextVersion != null)
            {
                lastnextVersion.TrangThaiApDung = true;
                await LuuLichSu("Edit", "Sửa trạng thái áp dụng version " + lastnextVersion.Version + " phần mềm" + lastnextVersion.SoftName.Name.ToUpper() + " thuộc Model: " + softName.Model.Name.ToString() + " thành Đang áp dụng");
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> checkToChangeNameSoft(int softId)
        {
            var check = await _context.Softwares.Where(x => x.SoftNameId == softId).CountAsync();
            if (check > 0)
            {
                return Json(new { success = false, mess = "Đã có dữ liệu" });
            }
            return Json(new { success = true, mess = "Có thể đổi tên" });
        }

        [HttpPost]
        public async Task<IActionResult> changeNameSoft(int softId, string tenMoi)
        { 
            var soft = await _context.SoftNames.Where(x => x.Id == softId).FirstOrDefaultAsync();
            if(soft != null)
            {
                soft.Name = tenMoi;
                await LuuLichSu("Edit", "Sửa tên phần mềm " + tenMoi);
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }
    }
}
