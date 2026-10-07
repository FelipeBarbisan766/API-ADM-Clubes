using API_PI_ADM_Clubes.Application.DTOs;
using API_PI_ADM_Clubes.Application.Interfaces.IRepositories;
using API_PI_ADM_Clubes.Infrastructure.Data;
using API_PI_ADM_Clubes.Model;
using Microsoft.EntityFrameworkCore;

namespace API_PI_ADM_Clubes.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdIncludingInactiveAsync(Guid id, CancellationToken ct)
            => await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

        public async Task<(IEnumerable<User> Items, int Total)> GetPagedAsync(UserFilterDTO f, CancellationToken ct)
        {
            var query = _context.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(f.Search))
                query = query.Where(u => u.Name.Contains(f.Search) || u.Email.Contains(f.Search));
            if (f.Role.HasValue) query = query.Where(u => u.Role == f.Role);
            if (f.IsActive.HasValue) query = query.Where(u => u.IsActive == f.IsActive);

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(u => u.Name)
                .Skip((f.Page - 1) * f.PageSize)
                .Take(f.PageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public async Task<bool> ExistsByCpfHashAsync(string cpfHash, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(u => u.CpfHash == cpfHash, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}