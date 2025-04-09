using InternshipManagementSystem.Application.DTOs;
using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InternshipManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.MatKhau))
            {
                return BadRequest("Email và mật khẩu là bắt buộc.");
            }

            string role = null;
            object user = null;

            // Kiểm tra email trong bảng SinhViens
            user = await _unitOfWork.SinhViens.GetAllAsync()
                .FirstOrDefaultAsync(s => s.Email == loginDTO.Email && s.MatKhau == loginDTO.MatKhau);
            if (user != null)
            {
                role = "SinhVien";
            }

            // Nếu không tìm thấy trong SinhViens, kiểm tra trong DoanhNghieps
            if (user == null)
            {
                user = await _unitOfWork.DoanhNghieps.GetAllAsync()
                    .FirstOrDefaultAsync(d => d.Email == loginDTO.Email && d.MatKhau == loginDTO.MatKhau);
                if (user != null)
                {
                    role = "DoanhNghiep";
                }
            }

            // Nếu không tìm thấy trong DoanhNghieps, kiểm tra trong Admins
            if (user == null)
            {
                user = await _unitOfWork.Admins.GetAllAsync()
                    .FirstOrDefaultAsync(a => a.Email == loginDTO.Email && a.MatKhau == loginDTO.MatKhau);
                if (user != null)
                {
                    role = "Admin";
                }
            }

            if (user == null)
            {
                return Unauthorized("Email hoặc mật khẩu không đúng.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, loginDTO.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = role // Trả về role để frontend điều hướng
            });
        }
    }
}