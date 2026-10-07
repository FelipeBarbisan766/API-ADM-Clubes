using API_PI_ADM_Clubes.Application.Interfaces.IRepositories;
using API_PI_ADM_Clubes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API_PI_ADM_Clubes.Infrastructure.Repositories;

public class ManagerRepository : IManagerRepository
{
    private readonly AppDbContext _context;

    public ManagerRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Manager?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Managers
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}