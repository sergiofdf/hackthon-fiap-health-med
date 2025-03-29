using Domain.Enums;

namespace Domain.Dto;

public record DoctorDto(
    string Id, 
    string Name, 
    string LastName, 
    string Email, 
    string Crm, 
    Specialties Specialty
    );