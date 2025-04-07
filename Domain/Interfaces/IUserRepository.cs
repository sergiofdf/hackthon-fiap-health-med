using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<bool> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string email, CancellationToken cancellationToken = default);
    Task<List<User>?> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByCrmAsync(string crm, CancellationToken cancellationToken = default);
    Task<User?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
}