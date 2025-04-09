using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using InternshipManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipManagementSystem.Infrastructure.Repositories
{
    public class ViTriThucTapRepository : IViTriThucTapRepository
    {
        private readonly ApplicationDbContext _context;

        public ViTriThucTapRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ViTriThucTap> AddAsync(ViTriThucTap vt)
        {
            vt.VtId = Guid.NewGuid();
            _context.ViTriThucTaps.Add(vt);
            await _context.SaveChangesAsync();
            return vt;
        }

        public async Task DeleteAsync(Guid id)
        {
            var vt = await _context.ViTriThucTaps.FindAsync(id);
            if (vt != null)
            {
                _context.ViTriThucTaps.Remove(vt);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ViTriThucTap>> GetByDoanhNghiepIdAsync(Guid dnId)
        {
            return await _context.ViTriThucTaps
                .Where(v => v.DnId == dnId)
                .ToListAsync();
        }

        public async Task<ViTriThucTap> GetByIdAsync(Guid id)
        {
            return await _context.ViTriThucTaps.FindAsync(id);
        }

        public async Task UpdateAsync(ViTriThucTap updated)
        {
            var vt = await _context.ViTriThucTaps.FindAsync(updated.VtId);
            if (vt != null)
            {
                vt.TenViTri = updated.TenViTri;
                vt.MoTa = updated.MoTa;
                vt.SoLuongTuyen = updated.SoLuongTuyen;
                vt.YeuCau = updated.YeuCau;

                await _context.SaveChangesAsync();
            }
        }
    }
}
