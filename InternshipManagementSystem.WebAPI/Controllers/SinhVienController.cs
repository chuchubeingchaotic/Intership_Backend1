﻿using InternshipManagementSystem.Application.DTOs;
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
    public class SinhVienController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SinhVienController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sinhViens = await _unitOfWork.SinhViens.GetAllAsync().ToListAsync();
            var sinhVienDTOs = sinhViens.Select(sv => new SinhVienDTO
            {
                SvId = sv.SvId,
                HoTen = sv.HoTen,
                MaSV = sv.MaSV,
                Lop = sv.Lop,
                Email = sv.Email,
                SDT = sv.SDT,
                GPA = sv.GPA
            }).ToList();
            return Ok(sinhVienDTOs);
        }

        [HttpPost]
        [AllowAnonymous] // Cho phép truy cập mà không cần xác thực
        public async Task<IActionResult> Create([FromBody] SinhVienDTO sinhVienDTO)
        {
            var existingSinhVien = await _unitOfWork.SinhViens.FirstOrDefaultAsync(s => s.Email == sinhVienDTO.Email || s.MaSV == sinhVienDTO.MaSV);
            if (existingSinhVien != null)
            {
                return BadRequest("Email hoặc Mã sinh viên đã tồn tại.");
            }

            var sinhVien = new SinhVien
            {
                SvId = Guid.NewGuid(),
                HoTen = sinhVienDTO.HoTen,
                MaSV = sinhVienDTO.MaSV,
                Lop = sinhVienDTO.Lop,
                Email = sinhVienDTO.Email,
                MatKhau = sinhVienDTO.MatKhau,
                SDT = sinhVienDTO.SDT,
                GPA = sinhVienDTO.GPA
            };

            await _unitOfWork.SinhViens.AddAsync(sinhVien);
            await _unitOfWork.CompleteAsync();
            return Ok(sinhVienDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SinhVienDTO sinhVienDTO)
        {
            var sinhVien = await _unitOfWork.SinhViens.GetByIdAsync(id);
            if (sinhVien == null) return NotFound();

            sinhVien.HoTen = sinhVienDTO.HoTen;
            sinhVien.MaSV = sinhVienDTO.MaSV;
            sinhVien.Lop = sinhVienDTO.Lop;
            sinhVien.Email = sinhVienDTO.Email;
            sinhVien.MatKhau = sinhVienDTO.MatKhau;
            sinhVien.SDT = sinhVienDTO.SDT;
            sinhVien.GPA = sinhVienDTO.GPA;

            await _unitOfWork.SinhViens.UpdateAsync(sinhVien);
            await _unitOfWork.CompleteAsync();
            return Ok(sinhVienDTO);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sinhVien = await _unitOfWork.SinhViens.GetByIdAsync(id);
            if (sinhVien == null) return NotFound();

            await _unitOfWork.SinhViens.DeleteAsync(sinhVien);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}