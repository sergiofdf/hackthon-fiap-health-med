using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IDoctorRepository
{
    Task<List<Doctor>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Doctor>> GetBySpecialityAsync(Specialties specialty, CancellationToken cancellationToken = default);
}