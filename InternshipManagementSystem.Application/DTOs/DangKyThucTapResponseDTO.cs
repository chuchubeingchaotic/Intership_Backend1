using System;

namespace InternshipManagementSystem.Application.DTOs
{
    public class DangKyThucTapResponseDTO
    {
        public Guid DkttId { get; set; }
        public SinhVienDTO SinhVien { get; set; } // Sử dụng SinhVienDTO đã có
        public DoanhNghiepDTO DoanhNghiep { get; set; } // Sử dụng DoanhNghiepDTO đã có
        public DateTime NgayDangKy { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}