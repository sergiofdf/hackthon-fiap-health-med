using Domain.Enums;

namespace Domain.Dto;

public record DoctorAgendaDto(
    string Id, 
    bool Available,
    DateTime StartTime,
    DateTime EndTime,
    string Name, 
    string LastName, 
    string Email, 
    string Crm, 
    Specialties Specialty,
    decimal HourlyPrice);
    