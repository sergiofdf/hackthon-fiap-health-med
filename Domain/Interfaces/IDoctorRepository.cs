using Domain.Entities;
using Domain.Enums;

namespace Domain.Interfaces;

public interface IDoctorRepository
{
    Task<List<Doctor>> GetAllAsync();
    Task<List<Doctor>> GetBySpecialityAsync(Specialties specialty);
}