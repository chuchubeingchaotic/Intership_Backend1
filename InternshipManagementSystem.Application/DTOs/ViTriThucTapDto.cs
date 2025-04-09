using System;

namespace InternshipManagementSystem.Application.DTOs
{
    public class ViTriThucTapDto
    {
        public Guid VtId { get; set; } // Thêm VtId
        public Guid DoanhNghiepId { get; set; }
        public string TenViTri { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public int SoLuong { get; set; } 
        public string YeuCau { get; set; } = string.Empty;
    }
}