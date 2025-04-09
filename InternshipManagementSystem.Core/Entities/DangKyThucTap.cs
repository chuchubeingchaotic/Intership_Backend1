using System;

namespace InternshipManagementSystem.Core.Entities
{
    public class DangKyThucTap
    {
        public Guid DkttId { get; set; }
        public Guid SvId { get; set; }
        public SinhVien SinhVien { get; set; }
        public Guid DnId { get; set; }
        public DoanhNghiep DoanhNghiep { get; set; }
        public Guid? VtId { get; set; } 
        public DateTime NgayDangKy { get; set; }
        public string TrangThai { get; set; }
    }
}