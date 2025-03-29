using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Repository;

public class DoctorRepository : IDoctorRepository
{
    private readonly ILogger<DoctorRepository> _logger;
    protected AppDbContext _dbContext;

    public DoctorRepository(ILogger<DoctorRepository> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task<List<Doctor>> GetAllAsync()
    {
        return await _dbContext.Doctors.ToListAsync();
    }

    public async Task<List<Doctor>> GetBySpecialityAsync(Specialties specialty)
    {
        return await _dbContext.Doctors
            .Where(d => d.Specialty == specialty)
            .ToListAsync();
    }
}