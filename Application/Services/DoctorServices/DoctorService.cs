using Domain.Dto;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services.DoctorServices;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly ILogger<DoctorService> _logger;

    public DoctorService(IDoctorRepository doctorRepository, ILogger<DoctorService> logger)
    {
        _doctorRepository = doctorRepository;
        _logger = logger;
    }
    
    public async Task<List<DoctorDto>> GetAllAsync()
    {
        var doctorsData = await _doctorRepository.GetAllAsync();

        return doctorsData.Select(doctor => new DoctorDto(doctor.Id, doctor.Name, doctor.LastName, doctor.Email, doctor.Crm, doctor.Specialty)).ToList();
    }

    public async Task<List<DoctorDto>> GetBySpecialtyAsync(Specialties specialty)
    {
        var doctorsData = await _doctorRepository.GetBySpecialityAsync(specialty);
        return doctorsData.Select(doctor => new DoctorDto(doctor.Id, doctor.Name, doctor.LastName, doctor.Email, doctor.Crm, doctor.Specialty)).ToList();
    }
}