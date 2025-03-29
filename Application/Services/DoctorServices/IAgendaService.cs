using Domain.Dto;
using Domain.Entities;

namespace Application.Services.DoctorServices;

public interface IAgendaService
{
    Task<bool> AddNewAvailableAgenda(string doctorId, DateTime startDateTime, DateTime endDateTime);
    Task<bool> UpdateAgenda(string agendaId, bool available, DateTime? startDateTime, DateTime? endDateTime);
    Task<List<DoctorAgendaDto>> GetDoctorAvailableAgendaByTime(string doctorId, DateTime startQueryDate,
        DateTime endQueryDate);
}