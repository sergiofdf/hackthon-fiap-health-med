using Domain.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infra.Repository;

public class AgendaRepository(AppDbContext dbContext) : IAgendaRepository
{
    public async Task<Agenda?> GetAgendaById(string agendaId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Agendas.FindAsync([agendaId, cancellationToken], cancellationToken);
    }

    public async Task<bool> AddAvailableSlotAsync(Agenda newAgenda, CancellationToken cancellationToken = default)
    {
        var doctor = await dbContext.Doctors.FindAsync([newAgenda.DoctorId, cancellationToken], cancellationToken);
        if (doctor == null) return false;
        
        await dbContext.Agendas.AddAsync(newAgenda, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<DoctorAgendaDto>> GetAvailableSlotsAsync(string doctorId, DateTime startQueryDate,
        DateTime endQueryDate, CancellationToken cancellationToken = default)
    {
        return await dbContext.Agendas
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
                a.Doctor.Specialty,
                a.Doctor.HourlyPrice
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> EditSlotAsync(Agenda newAgenda, CancellationToken cancellationToken = default)
    {
        var slot = await dbContext.Agendas.FindAsync([newAgenda.Id], cancellationToken);
        if (slot == null) return false;
        
        slot.Available = newAgenda.Available;
        slot.StartTime = newAgenda.StartTime;
        slot.EndTime = newAgenda.EndTime;
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}