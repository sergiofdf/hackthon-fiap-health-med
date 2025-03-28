using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Repository;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    protected AppDbContext _dbContext;

    public UserRepository(ILogger<UserRepository> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<bool> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var userUpdated = _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return userUpdated.Entity;
    }

    public async Task<bool> DeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        var userToDelete = await GetByEmailAsync(email, cancellationToken);
        if (userToDelete == null) return false;
        _dbContext.Users.Remove(userToDelete);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByCrmAsync(string crm, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .OfType<Doctor>()
            .FirstOrDefaultAsync(d => d.Crm == crm, cancellationToken: cancellationToken);
    }

    public async Task<User?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .OfType<Patient>()
            .FirstOrDefaultAsync(d => d.Cpf == cpf, cancellationToken: cancellationToken);
    }
}