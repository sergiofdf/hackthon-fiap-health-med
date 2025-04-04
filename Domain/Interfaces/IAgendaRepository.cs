using Domain.Dto;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IAgendaRepository
{
    Task<Agenda?> GetAgendaById(string agendaId, CancellationToken cancellationToken = default);
    Task<bool> AddAvailableSlotAsync(Agenda newAgenda, CancellationToken cancellationToken = default);
    Task<List<DoctorAgendaDto>> GetAvailableSlotsAsync(string doctorId, DateTime startQueryDate, DateTime endQueryDate, CancellationToken cancellationToken = default);
    Task<bool> EditSlotAsync(Agenda newAgenda, CancellationToken cancellationToken = default);
}