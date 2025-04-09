using System;

namespace InternshipManagementSystem.Application.DTOs
{
    public class DangKyThucTapDTO
    {
        public Guid DkttId { get; set; }
        public Guid SvId { get; set; }
        public SinhVienDTO SinhVien { get; set; }
        public Guid DnId { get; set; }
        public DoanhNghiepDTO DoanhNghiep { get; set; }
        public Guid? VtId { get; set; } 
        public ViTriThucTapDto ViTriThucTap { get; set; }
        public DateTime NgayDangKy { get; set; }
        public string TrangThai { get; set; }
        public string Email { get; set; }
    }
}