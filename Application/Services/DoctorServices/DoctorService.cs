using Domain.Dto;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services.DoctorServices;

public class DoctorService(IDoctorRepository doctorRepository) : IDoctorService
{
    public async Task<List<DoctorDto>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var doctorsData = await doctorRepository.GetAllAsync(page, pageSize, cancellationToken);

        return doctorsData.Select(doctor => new DoctorDto(doctor.Id, doctor.Name, doctor.LastName, doctor.Email, doctor.Crm, doctor.Specialty)).ToList();
    }

    public async Task<List<DoctorDto>> GetBySpecialtyAsync(Specialties specialty, CancellationToken cancellationToken = default)
    {
        var doctorsData = await doctorRepository.GetBySpecialityAsync(specialty, cancellationToken);
        return doctorsData.Select(doctor => new DoctorDto(doctor.Id, doctor.Name, doctor.LastName, doctor.Email, doctor.Crm, doctor.Specialty)).ToList();
    }
}