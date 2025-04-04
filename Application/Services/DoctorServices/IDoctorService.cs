using Domain.Dto;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.DoctorServices;

public interface IDoctorService
{
    Task<List<DoctorDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<DoctorDto>> GetBySpecialtyAsync(Specialties specialty, CancellationToken cancellationToken = default);
}