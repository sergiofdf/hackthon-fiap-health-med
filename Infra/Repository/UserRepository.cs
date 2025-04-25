using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Repository;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<bool> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var userUpdated = dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return userUpdated.Entity;
    }

    public async Task<bool> DeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        var userToDelete = await GetByEmailAsync(email, cancellationToken);
        if (userToDelete == null) return false;
        dbContext.Users.Remove(userToDelete);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<User>?> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .OrderBy(u => u.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByCrmAsync(string crm, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .OfType<Doctor>()
            .FirstOrDefaultAsync(d => d.Crm == crm, cancellationToken: cancellationToken);
    }

    public async Task<User?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .OfType<Patient>()
            .FirstOrDefaultAsync(d => d.Cpf == cpf, cancellationToken: cancellationToken);
    }
}