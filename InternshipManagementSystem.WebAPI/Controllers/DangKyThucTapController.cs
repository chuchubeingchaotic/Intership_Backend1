using InternshipManagementSystem.Application.DTOs;
using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Thêm using này

namespace InternshipManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DangKyThucTapController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DangKyThucTapController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dangKyThucTaps = await _unitOfWork.DangKyThucTaps
                .GetAllAsync()
                .Include(dk => dk.SinhVien) // Tải SinhVien
                .Include(dk => dk.DoanhNghiep) // Tải DoanhNghiep
                .ToListAsync();

            var dangKyThucTapDTOs = dangKyThucTaps.Select(dk => new DangKyThucTapResponseDTO
            {
                DkttId = dk.DkttId,
                SinhVien = dk.SinhVien != null ? new SinhVienDTO
                {
                    SvId = dk.SinhVien.SvId,
                    HoTen = dk.SinhVien.HoTen,
                    MaSV = dk.SinhVien.MaSV,
                    Lop = dk.SinhVien.Lop,
                    Email = dk.SinhVien.Email,
                    SDT = dk.SinhVien.SDT,
                    GPA = dk.SinhVien.GPA
                } : null,
                DoanhNghiep = dk.DoanhNghiep != null ? new DoanhNghiepDTO
                {
                    DnId = dk.DoanhNghiep.DnId,
                    TenDN = dk.DoanhNghiep.TenDN,
                    DiaChi = dk.DoanhNghiep.DiaChi,
                    SoDT = dk.DoanhNghiep.SoDT,
                    Email = dk.DoanhNghiep.Email
                } : null,
                NgayDangKy = dk.NgayDangKy,
                TrangThai = dk.TrangThai
            }).ToList();
            return Ok(dangKyThucTapDTOs);
        }

        // Các phương thức khác giữ nguyên
        [HttpPost]
        [Authorize(Roles = "SinhVien")]
        public async Task<IActionResult> Create([FromBody] DangKyThucTapDTO dangKyThucTapDTO)
        {
            var sinhVien = await _unitOfWork.SinhViens.FirstOrDefaultAsync(s => s.Email == dangKyThucTapDTO.Email);
            if (sinhVien == null) return BadRequest("Email không hợp lệ hoặc không tồn tại.");

            var dangKyThucTap = new DangKyThucTap
            {
                DkttId = Guid.NewGuid(),
                SvId = sinhVien.SvId,
                DnId = dangKyThucTapDTO.DnId,
                NgayDangKy = DateTime.Now,
                TrangThai = "Chờ duyệt"
            };

            await _unitOfWork.DangKyThucTaps.AddAsync(dangKyThucTap);
            await _unitOfWork.CompleteAsync();

            var responseDTO = new DangKyThucTapResponseDTO
            {
                DkttId = dangKyThucTap.DkttId,
                SinhVien = new SinhVienDTO
                {
                    SvId = sinhVien.SvId,
                    HoTen = sinhVien.HoTen,
                    MaSV = sinhVien.MaSV,
                    Lop = sinhVien.Lop,
                    Email = sinhVien.Email,
                    SDT = sinhVien.SDT,
                    GPA = sinhVien.GPA
                },
                DoanhNghiep = await _unitOfWork.DoanhNghieps.GetByIdAsync(dangKyThucTap.DnId) is DoanhNghiep dn ? new DoanhNghiepDTO
                {
                    DnId = dn.DnId,
                    TenDN = dn.TenDN,
                    DiaChi = dn.DiaChi,
                    SoDT = dn.SoDT,
                    Email = dn.Email
                } : null,
                NgayDangKy = dangKyThucTap.NgayDangKy,
                TrangThai = dangKyThucTap.TrangThai
            };

            return Ok(responseDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "DoanhNghiep,Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DangKyThucTapDTO dangKyThucTapDTO)
        {
            var dangKyThucTap = await _unitOfWork.DangKyThucTaps
                .GetAllAsync()
                .Include(dk => dk.SinhVien)
                .Include(dk => dk.DoanhNghiep)
                .FirstOrDefaultAsync(dk => dk.DkttId == id);

            if (dangKyThucTap == null) return NotFound();

            dangKyThucTap.TrangThai = dangKyThucTapDTO.TrangThai;

            await _unitOfWork.DangKyThucTaps.UpdateAsync(dangKyThucTap);
            await _unitOfWork.CompleteAsync();

            var responseDTO = new DangKyThucTapResponseDTO
            {
                DkttId = dangKyThucTap.DkttId,
                SinhVien = dangKyThucTap.SinhVien != null ? new SinhVienDTO
                {
                    SvId = dangKyThucTap.SinhVien.SvId,
                    HoTen = dangKyThucTap.SinhVien.HoTen,
                    MaSV = dangKyThucTap.SinhVien.MaSV,
                    Lop = dangKyThucTap.SinhVien.Lop,
                    Email = dangKyThucTap.SinhVien.Email,
                    SDT = dangKyThucTap.SinhVien.SDT,
                    GPA = dangKyThucTap.SinhVien.GPA
                } : null,
                DoanhNghiep = dangKyThucTap.DoanhNghiep != null ? new DoanhNghiepDTO
                {
                    DnId = dangKyThucTap.DoanhNghiep.DnId,
                    TenDN = dangKyThucTap.DoanhNghiep.TenDN,
                    DiaChi = dangKyThucTap.DoanhNghiep.DiaChi,
                    SoDT = dangKyThucTap.DoanhNghiep.SoDT,
                    Email = dangKyThucTap.DoanhNghiep.Email
                } : null,
                NgayDangKy = dangKyThucTap.NgayDangKy,
                TrangThai = dangKyThucTap.TrangThai
            };

            return Ok(responseDTO);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dangKyThucTap = await _unitOfWork.DangKyThucTaps.GetByIdAsync(id);
            if (dangKyThucTap == null) return NotFound();

            await _unitOfWork.DangKyThucTaps.DeleteAsync(dangKyThucTap);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpGet("doanh-nghieps")]
        public async Task<IActionResult> GetAllDoanhNghieps()
        {
            var doanhNghieps = await _unitOfWork.DoanhNghieps.GetAllAsync().ToListAsync(); // Thêm ToListAsync()
            var doanhNghiepDTOs = doanhNghieps.Select(dn => new DoanhNghiepDTO
            {
                DnId = dn.DnId,
                TenDN = dn.TenDN,
                DiaChi = dn.DiaChi,
                SoDT = dn.SoDT,
                Email = dn.Email
            }).ToList();
            return Ok(doanhNghiepDTOs);
        }
    }
}