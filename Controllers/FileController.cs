using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftVersionControl.Models;
using System.Globalization;

namespace SoftVersionControl.Controllers
{
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly AppDbContext _context;
        public FileController(ILogger<FileController> logger, AppDbContext context)
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
        [DisableRequestSizeLimit]
        [Route("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "temps");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                // Kết hợp đường dẫn thư mục và tên tệp tin gốc để có đường dẫn đầy đủ.
                string fileName = Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);
                //string uniqueFileName = originalFileName + "_" + Guid.NewGuid().ToString() + fileExtension;
                if (System.IO.File.Exists(filePath))
                {
                    return BadRequest("Tệp tin đã tồn tại với cùng tên. Hãy xoá file trong /upload/temps và thử lại");
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                await LuuLichSu("Upload", "Tải lên file: " + fileName);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateFolder(string folderPathModel, string folderPathSoft)
        {
            try
            {
                string newUploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderPathModel, folderPathSoft);
                if (!Directory.Exists(newUploadFolder))
                {
                    Directory.CreateDirectory(newUploadFolder);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CutAndPasteFile(string modelName, string softName, string fileName, string version)
        {
            try
            {
                string newFileName = version + "_" + fileName;
                string sourceFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "temps", fileName);
                string destinationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", modelName, softName, newFileName);
                if (System.IO.File.Exists(sourceFilePath))
                {
                    System.IO.File.Move(sourceFilePath, destinationFilePath);
                    return Json(new { success = true, message = "File đã được cắt và dán thành công." });
                }
                else
                {
                    return Json(new { success = false, message = "File nguồn không tồn tại." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadInfoSoft([FromBody] UploadInfoSoft formData)
        {
            string format = "yyyy/MM/dd";
            try
            {
                int softNameId = Convert.ToInt32(formData.SoftNameId);
                //ISession session = HttpContext.Session;
                var newSoft = new Software();
                newSoft.Version = formData.Version;
                newSoft.DiemThayDoi = formData.DiemThayDoi;
                if (DateTime.TryParseExact(formData.NgayApDung, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultNgayApDung))
                {
                    newSoft.NgayApDung = resultNgayApDung;
                }
                if (DateTime.TryParseExact(formData.NgayCaiDat, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultNgayCaiDat))
                {
                    newSoft.NgayCaiDat = resultNgayCaiDat;
                }
                newSoft.NguoiCaiDat = "Sang Nguyen"; //phai lay tu session
                newSoft.NguoiXacNhan = formData.NguoiXacNhan;
                newSoft.SoLuongJigCaiDat = Convert.ToInt32(formData.SoLuongJigCaiDat);
                newSoft.TrangThaiApDung = true;
                newSoft.FileName = formData.FileName;
                newSoft.Path = formData.Path;
                newSoft.TimeCreated = DateTime.Now;
                newSoft.SoftNameId = softNameId;
                _context.Softwares.Add(newSoft);
                await _context.SaveChangesAsync();

                string tenphanmem = _context.SoftNames.Where(x => x.Id == softNameId).FirstOrDefault().Name.ToString();
                await LuuLichSu("Create", "Cập nhật phiên bản mới phần mềm: " + tenphanmem + " , Đường dẫn: " + formData.Path);
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully, log them, and return an error response
                return Json(new { success = false, error = ex.Message });
            }
        }

        public IActionResult DownloadFile(string filename)
        {
            // Đường dẫn thực tế đến file trên máy chủ
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filename);

            // Kiểm tra xem file có tồn tại không
            if (System.IO.File.Exists(filePath))
            {
                // Đọc nội dung của file thành một mảng byte
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                // Xác định loại nội dung của file
                string contentType = "application/octet-stream"; // Loại nội dung tổng quát cho các tệp tin nhị phân
                // Tạo một tên file để hiển thị khi tải về
                string downloadFileName = filename; // Bạn có thể điều chỉnh tên file theo ý muốn
                LuuLichSu("Download", "Tải về phần mềm: " + filename);
                return File(fileBytes, contentType, downloadFileName);
            }
            else
            {
                return NotFound();
            }
        }


    }
}