using System.Globalization;
using Application.Models;
using Domain.Dto;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.Services.DoctorServices;

public class AgendaService(IAgendaRepository agendaRepository) : IAgendaService
{
    public async Task<bool> AddNewAvailableAgenda(string doctorId, DateTime startDateTime, DateTime endDateTime, CancellationToken ct = default)
    {
        var doctorAvaiableAgendas = await GetDoctorAvailableAgendaByTime(doctorId, startDateTime, endDateTime, ct);

        if (doctorAvaiableAgendas.Count > 0)
        {
            Field field = new()
            {
                Name = "startDateTime",
                Value = startDateTime.ToString(CultureInfo.InvariantCulture),
                ExMessage = "Horário livre já cadastrado."
            };
            List<Field> fields = new() { field };
                
            DataValidationException.Throw("400", "Erro no registro de agenda", "Agenda já cadastrada.", fields);
        }
        
        var newAgenda = new Agenda
        {
            StartTime = startDateTime,
            EndTime = endDateTime,
            Available = true,
            DoctorId = doctorId,
        };
        
        var res = await agendaRepository.AddAvailableSlotAsync(newAgenda, ct);

        if (!res)
        {
            NotFoundException.Throw("404", "Médico não encontrado. Confira o id informado.");
        }

        return res;
    }
    
    public async Task<bool> UpdateAgenda(string agendaId, UpdateAgendaDto updateAgendaDto, CancellationToken ct = default)
    {
        var agenda = await agendaRepository.GetAgendaById(agendaId, ct);
        
        if (agenda == null)
        {
            NotFoundException.Throw("404", "Agenda não encontrada.");
        }

        var startDateTimeUtc = updateAgendaDto.StartDateTime is not null
            ? DateTime.SpecifyKind((DateTime) updateAgendaDto.StartDateTime, DateTimeKind.Utc)
            : agenda!.StartTime;
        
        var endDateTimeUtc = updateAgendaDto.EndDateTime is not null
            ? DateTime.SpecifyKind((DateTime) updateAgendaDto.EndDateTime, DateTimeKind.Utc)
            : agenda!.EndTime;

        var newAgenda = new Agenda
        {
            Id = agendaId,
            Available = updateAgendaDto.Available,
            StartTime = startDateTimeUtc,
            EndTime = endDateTimeUtc,
            DoctorId = agenda!.DoctorId
        };
        
        var res = await agendaRepository.EditSlotAsync(newAgenda, ct);
        
        return res;
    }
    public async Task<List<DoctorAgendaDto>> GetDoctorAvailableAgendaByTime(string doctorId, DateTime startQueryDate, DateTime endQueryDate, CancellationToken ct = default)
    {
        return await agendaRepository.GetAvailableSlotsAsync(doctorId, startQueryDate, endQueryDate, ct);
    }
}