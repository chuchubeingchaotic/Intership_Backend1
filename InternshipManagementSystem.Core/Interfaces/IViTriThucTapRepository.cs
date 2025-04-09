using InternshipManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipManagementSystem.Core.Interfaces
{
    public interface IViTriThucTapRepository
    {
        Task<IEnumerable<ViTriThucTap>> GetByDoanhNghiepIdAsync(Guid dnId);
        Task<ViTriThucTap> GetByIdAsync(Guid id);
        Task<ViTriThucTap> AddAsync(ViTriThucTap vt);
        Task UpdateAsync(ViTriThucTap vt);
        Task DeleteAsync(Guid id);
    }
}
