using InternshipManagementSystem.Application.DTOs;
using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        [HttpGet("doanh-nghieps")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoanhNghieps()
        {
            var doanhNghieps = await _unitOfWork.DoanhNghieps.GetAllAsync()
                .Select(dn => new DoanhNghiepDTO
                {
                    DnId = dn.DnId,
                    TenDN = dn.TenDN,
                    DiaChi = dn.DiaChi,
                    SoDT = dn.SoDT,
                    Email = dn.Email,
                    ViTriThucTaps = dn.ViTriThucTaps.Select(v => new ViTriThucTapDto
                    {
                        VtId = v.VtId,
                        DoanhNghiepId = v.DnId,
                        TenViTri = v.TenViTri,
                        MoTa = v.MoTa,
                        SoLuong = v.SoLuongTuyen,
                        YeuCau = v.YeuCau ?? "Không có yêu cầu"
                    }).ToList()
                })
                .ToListAsync();

            return Ok(doanhNghieps);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dangKyThucTaps = await _unitOfWork.DangKyThucTaps.GetAllAsync()
                .Include(dk => dk.SinhVien)
                .Include(dk => dk.DoanhNghiep)
                .ToListAsync();

            var viTriThucTaps = await _unitOfWork.ViTriThucTaps.GetAllAsync()
                .ToDictionaryAsync(v => v.VtId, v => v);

            var dangKyThucTapDtos = dangKyThucTaps.Select(dk =>
            {
                ViTriThucTap viTri = null;
                if (dk.VtId.HasValue && viTriThucTaps.ContainsKey(dk.VtId.Value))
                {
                    viTri = viTriThucTaps[dk.VtId.Value];
                }

                return new DangKyThucTapDTO
                {
                    DkttId = dk.DkttId,
                    SvId = dk.SvId,
                    SinhVien = new SinhVienDTO
                    {
                        SvId = dk.SinhVien.SvId,
                        HoTen = dk.SinhVien.HoTen,
                        MaSV = dk.SinhVien.MaSV,
                        Lop = dk.SinhVien.Lop,
                        Email = dk.SinhVien.Email,
                        SDT = dk.SinhVien.SDT,
                        GPA = dk.SinhVien.GPA
                    },
                    DnId = dk.DnId,
                    DoanhNghiep = new DoanhNghiepDTO
                    {
                        DnId = dk.DoanhNghiep.DnId,
                        TenDN = dk.DoanhNghiep.TenDN,
                        DiaChi = dk.DoanhNghiep.DiaChi,
                        SoDT = dk.DoanhNghiep.SoDT,
                        Email = dk.DoanhNghiep.Email
                    },
                    VtId = dk.VtId,
                    ViTriThucTap = viTri != null ? new ViTriThucTapDto
                    {
                        VtId = viTri.VtId,
                        DoanhNghiepId = viTri.DnId,
                        TenViTri = viTri.TenViTri,
                        MoTa = viTri.MoTa,
                        SoLuong = viTri.SoLuongTuyen,
                        YeuCau = viTri.YeuCau ?? "Không có yêu cầu"
                    } : null,
                    NgayDangKy = dk.NgayDangKy,
                    TrangThai = dk.TrangThai
                };
            }).ToList();

            return Ok(dangKyThucTapDtos);
        }

        [HttpPost]
        [HttpPost]
        [Authorize(Roles = "SinhVien")]
        public async Task<IActionResult> Create([FromBody] DangKyThucTapCreateDTO dangKyDTO)
        {
            // Lấy email từ token
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            // Lấy thông tin sinh viên từ email
            var sinhVien = await _unitOfWork.SinhViens.FirstOrDefaultAsync(sv => sv.Email == email);
            if (sinhVien == null)
                return BadRequest("Sinh viên không tồn tại.");

            // Lấy vị trí thực tập
            var viTri = await _unitOfWork.ViTriThucTaps.GetByIdAsync(dangKyDTO.VtId);
            if (viTri == null)
                return BadRequest("Vị trí thực tập không tồn tại.");

            // Lấy doanh nghiệp của vị trí
            var doanhNghiep = await _unitOfWork.DoanhNghieps.GetByIdAsync(viTri.DnId);
            if (doanhNghiep == null)
                return BadRequest("Doanh nghiệp không tồn tại.");

            // Kiểm tra sinh viên đã đăng ký vị trí này chưa
            var existingDangKy = await _unitOfWork.DangKyThucTaps.GetAllAsync()
                .FirstOrDefaultAsync(dk => dk.SvId == sinhVien.SvId
                                           && dk.VtId == viTri.VtId
                                           && dk.TrangThai == "Chờ duyệt");
            if (existingDangKy != null)
                return BadRequest("Bạn đã đăng ký vị trí này và đang chờ duyệt.");

            // Tạo mới đăng ký
            var dangKy = new DangKyThucTap
            {
                DkttId = Guid.NewGuid(),
                SvId = sinhVien.SvId,
                DnId = viTri.DnId,
                VtId = viTri.VtId,
                NgayDangKy = DateTime.Now,
                TrangThai = "Chờ duyệt"
            };

            await _unitOfWork.DangKyThucTaps.AddAsync(dangKy);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Đăng ký thực tập thành công!", dangKyId = dangKy.DkttId });
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dangKy = await _unitOfWork.DangKyThucTaps.GetByIdAsync(id);
            if (dangKy == null) return NotFound();

            await _unitOfWork.DangKyThucTaps.DeleteAsync(dangKy);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "DoanhNghiep")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateDangKyStatusDTO statusDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var doanhNghiep = await _unitOfWork.DoanhNghieps.FirstOrDefaultAsync(d => d.Email == email);
            if (doanhNghiep == null)
            {
                return Unauthorized("Doanh nghiệp không tồn tại.");
            }

            var dangKy = await _unitOfWork.DangKyThucTaps.GetByIdAsync(id);
            if (dangKy == null)
            {
                return NotFound("Đơn đăng ký không tồn tại.");
            }

            if (dangKy.DnId != doanhNghiep.DnId)
            {
                return Forbid("Bạn không có quyền cập nhật trạng thái đơn này.");
            }

            if (statusDTO.TrangThai != "Đã duyệt" && statusDTO.TrangThai != "Từ chối")
            {
                return BadRequest("Trạng thái không hợp lệ. Chỉ chấp nhận 'Đã duyệt' hoặc 'Từ chối'.");
            }

            dangKy.TrangThai = statusDTO.TrangThai;
            await _unitOfWork.DangKyThucTaps.UpdateAsync(dangKy);
            await _unitOfWork.CompleteAsync();

            return Ok(new { DkttId = dangKy.DkttId, TrangThai = dangKy.TrangThai });
        }
    }
}