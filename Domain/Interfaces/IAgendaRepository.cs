using Domain.Entities;

namespace Domain.Interfaces;

public interface IAgendaRepository
{
    Task<Agenda?> GetAgendaById(string agendaId);
    Task<bool> AddAvailableSlotAsync(Agenda newAgenda);
    Task<List<Agenda>> GetAvailableSlotsAsync(string doctorId, DateTime startQueryDate, DateTime endQueryDate);
    Task<bool> EditSlotAsync(Agenda newAgenda);
}