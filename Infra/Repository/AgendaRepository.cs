using Domain.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Repository;

public class AgendaRepository : IAgendaRepository
{
    private readonly ILogger<AgendaRepository> _logger;
    protected AppDbContext _dbContext;

    public AgendaRepository(ILogger<AgendaRepository> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Agenda?> GetAgendaById(string agendaId)
    {
        return await _dbContext.Agendas.FindAsync(agendaId);
    }

    public async Task<bool> AddAvailableSlotAsync(Agenda newAgenda)
    {
        var doctor = await _dbContext.Doctors.FindAsync(newAgenda.DoctorId);
        if (doctor == null) return false;
        
        await _dbContext.Agendas.AddAsync(newAgenda);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<DoctorAgendaDto>> GetAvailableSlotsAsync(string doctorId, DateTime startQueryDate,
        DateTime endQueryDate)
    {
        return await _dbContext.Agendas
            .Where(a => a.DoctorId == doctorId && a.Available &&
                        a.StartTime >= startQueryDate && a.EndTime <= endQueryDate)
            .OrderBy(a => a.StartTime)
            .Include(a => a.Doctor)
            .Select(a => new DoctorAgendaDto(
                a.Id,
                a.Available,
                a.StartTime,
                a.EndTime,
                a.Doctor.Name,
                a.Doctor.LastName,
                a.Doctor.Email,
                a.Doctor.Crm,
                a.Doctor.Specialty
            ))
            .ToListAsync();
    }

    public async Task<bool> EditSlotAsync(Agenda newAgenda)
    {
        var slot = await _dbContext.Agendas.FindAsync(newAgenda.Id);
        if (slot == null) return false;
        
        _dbContext.Agendas.Update(newAgenda);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}