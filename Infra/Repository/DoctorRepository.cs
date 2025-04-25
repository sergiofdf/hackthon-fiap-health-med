using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Repository;

public class DoctorRepository(AppDbContext dbContext) : IDoctorRepository
{
    public async Task<List<Doctor>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await dbContext.Doctors
            .OrderBy(d => d.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Doctor>> GetBySpecialityAsync(Specialties specialty, CancellationToken cancellationToken = default)
    {
        return await dbContext.Doctors
            .Where(d => d.Specialty == specialty)
            .ToListAsync(cancellationToken);
    }
}