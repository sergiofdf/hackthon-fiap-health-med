using Domain.Dto;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.DoctorServices;

public interface IDoctorService
{
    Task<List<DoctorDto>> GetAllAsync();
    Task<List<DoctorDto>> GetBySpecialtyAsync(Specialties specialty);
}