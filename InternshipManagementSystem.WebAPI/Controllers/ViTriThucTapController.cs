using InternshipManagementSystem.Application.DTOs;
using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace InternshipManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "DoanhNghiep")]
    public class ViTriThucTapController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViTriThucTapController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var doanhNghiep = await _unitOfWork.DoanhNghieps.FirstOrDefaultAsync(d => d.Email == email);
            if (doanhNghiep == null)
            {
                return Unauthorized("Doanh nghiệp không tồn tại.");
            }

            // Sửa lỗi: Không await trực tiếp trên GetAllAsync, mà thực thi truy vấn trong một bước
            var viTriThucTaps = await _unitOfWork.ViTriThucTaps.GetAllAsync()
                .Where(v => v.DnId == doanhNghiep.DnId)
                .Select(v => new ViTriThucTapDto
                {
                    VtId = v.VtId,
                    DoanhNghiepId = v.DnId,
                    TenViTri = v.TenViTri,
                    MoTa = v.MoTa,
                    SoLuong = v.SoLuongTuyen,
                    YeuCau = v.YeuCau ?? "Không có yêu cầu"
                })
                .ToListAsync();

            return Ok(viTriThucTaps);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ViTriThucTapDto viTriDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var doanhNghiep = await _unitOfWork.DoanhNghieps.FirstOrDefaultAsync(d => d.Email == email);
            if (doanhNghiep == null)
            {
                return Unauthorized("Doanh nghiệp không tồn tại.");
            }

            var viTri = new ViTriThucTap
            {
                VtId = Guid.NewGuid(),
                DnId = doanhNghiep.DnId,
                TenViTri = viTriDTO.TenViTri,
                MoTa = viTriDTO.MoTa,
                SoLuongTuyen = viTriDTO.SoLuong,
                YeuCau = viTriDTO.YeuCau
            };

            await _unitOfWork.ViTriThucTaps.AddAsync(viTri);
            await _unitOfWork.CompleteAsync();

            viTriDTO.VtId = viTri.VtId;
            viTriDTO.DoanhNghiepId = viTri.DnId;
            return Ok(viTriDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ViTriThucTapDto viTriDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var doanhNghiep = await _unitOfWork.DoanhNghieps.FirstOrDefaultAsync(d => d.Email == email);
            if (doanhNghiep == null)
            {
                return Unauthorized("Doanh nghiệp không tồn tại.");
            }

            var viTri = await _unitOfWork.ViTriThucTaps.FirstOrDefaultAsync(v => v.VtId == id && v.DnId == doanhNghiep.DnId);
            if (viTri == null)
            {
                return NotFound("Vị trí thực tập không tồn tại hoặc không thuộc doanh nghiệp này.");
            }

            viTri.TenViTri = viTriDTO.TenViTri;
            viTri.MoTa = viTriDTO.MoTa;
            viTri.SoLuongTuyen = viTriDTO.SoLuong;
            viTri.YeuCau = viTriDTO.YeuCau;

            await _unitOfWork.ViTriThucTaps.UpdateAsync(viTri);
            await _unitOfWork.CompleteAsync();

            return Ok(viTriDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var doanhNghiep = await _unitOfWork.DoanhNghieps.FirstOrDefaultAsync(d => d.Email == email);
            if (doanhNghiep == null)
            {
                return Unauthorized("Doanh nghiệp không tồn tại.");
            }

            var viTri = await _unitOfWork.ViTriThucTaps.FirstOrDefaultAsync(v => v.VtId == id && v.DnId == doanhNghiep.DnId);
            if (viTri == null)
            {
                return NotFound("Vị trí thực tập không tồn tại hoặc không thuộc doanh nghiệp này.");
            }

            await _unitOfWork.ViTriThucTaps.DeleteAsync(viTri);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}