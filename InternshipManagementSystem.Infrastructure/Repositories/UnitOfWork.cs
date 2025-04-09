using InternshipManagementSystem.Core.Entities;
using InternshipManagementSystem.Core.Interfaces;
using InternshipManagementSystem.Infrastructure.Data; // Thêm namespace

namespace InternshipManagementSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<SinhVien> _sinhViens;
        private IRepository<DoanhNghiep> _doanhNghieps;
        private IRepository<DangKyThucTap> _dangKyThucTaps;
        private IRepository<ViTriThucTap> _viTriThucTaps;
        private IRepository<Admin> _admins; // Thêm field

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<SinhVien> SinhViens
        {
            get { return _sinhViens ??= new Repository<SinhVien>(_context); }
        }

        public IRepository<DoanhNghiep> DoanhNghieps
        {
            get { return _doanhNghieps ??= new Repository<DoanhNghiep>(_context); }
        }

        public IRepository<DangKyThucTap> DangKyThucTaps
        {
            get { return _dangKyThucTaps ??= new Repository<DangKyThucTap>(_context); }
        }

        public IRepository<ViTriThucTap> ViTriThucTaps
        {
            get { return _viTriThucTaps ??= new Repository<ViTriThucTap>(_context); }
        }

        public IRepository<Admin> Admins // Triển khai thuộc tính
        {
            get { return _admins ??= new Repository<Admin>(_context); }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}