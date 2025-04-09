using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternshipManagementSystem.Core.Entities
{
    public class ViTriThucTap
    {
        [Key]
        public Guid VtId { get; set; }

        [Required]
        public Guid DnId { get; set; }

        [Required]
        public string TenViTri { get; set; }

        public string MoTa { get; set; }

        public int SoLuongTuyen { get; set; }

        public string YeuCau { get; set; }

        [ForeignKey("DnId")]
        public DoanhNghiep DoanhNghiep { get; set; } 
    }
}
