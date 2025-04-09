using InternshipManagementSystem.Application.DTOs;
using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InternshipManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu xác thực cho toàn bộ controller
    public class DoanhNghiepController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoanhNghiepController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doanhNghieps = await _unitOfWork.DoanhNghieps.GetAllAsync().ToListAsync();
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

        [HttpPost]
        [AllowAnonymous] 
        public async Task<IActionResult> Create([FromBody] DoanhNghiepDTO doanhNghiepDTO)
        {
            var existingDoanhNghiep = await _unitOfWork.DoanhNghieps.FirstOrDefaultAsync(d => d.Email == doanhNghiepDTO.Email);
            if (existingDoanhNghiep != null)
            {
                return BadRequest("Email đã tồn tại.");
            }

            var doanhNghiep = new DoanhNghiep
            {
                DnId = Guid.NewGuid(),
                TenDN = doanhNghiepDTO.TenDN,
                DiaChi = doanhNghiepDTO.DiaChi,
                SoDT = doanhNghiepDTO.SoDT,
                Email = doanhNghiepDTO.Email,
                MatKhau = doanhNghiepDTO.MatKhau
            };

            await _unitOfWork.DoanhNghieps.AddAsync(doanhNghiep);
            await _unitOfWork.CompleteAsync();
            return Ok(doanhNghiepDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DoanhNghiepDTO doanhNghiepDTO)
        {
            var doanhNghiep = await _unitOfWork.DoanhNghieps.GetByIdAsync(id);
            if (doanhNghiep == null) return NotFound();

            doanhNghiep.TenDN = doanhNghiepDTO.TenDN;
            doanhNghiep.DiaChi = doanhNghiepDTO.DiaChi;
            doanhNghiep.SoDT = doanhNghiepDTO.SoDT;
            doanhNghiep.Email = doanhNghiepDTO.Email;
            doanhNghiep.MatKhau = doanhNghiepDTO.MatKhau;

            await _unitOfWork.DoanhNghieps.UpdateAsync(doanhNghiep);
            await _unitOfWork.CompleteAsync();
            return Ok(doanhNghiepDTO);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var doanhNghiep = await _unitOfWork.DoanhNghieps.GetByIdAsync(id);
            if (doanhNghiep == null) return NotFound();

            await _unitOfWork.DoanhNghieps.DeleteAsync(doanhNghiep);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}