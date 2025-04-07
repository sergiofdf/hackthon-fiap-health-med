using System.Globalization;
using Application.Models;
using Domain.Dto;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Services.DoctorServices;

public class AgendaService(IAgendaRepository agendaRepository) : IAgendaService
{
    public async Task<bool> AddNewAvailableAgenda(string doctorId, AgendaDto agendaDto, CancellationToken ct = default)
    {
        var startDateTimeUtc = DateTime.SpecifyKind((DateTime)agendaDto.StartDateTime!, DateTimeKind.Utc);
        var endDateTimeUtc = DateTime.SpecifyKind((DateTime)agendaDto.EndDateTime!, DateTimeKind.Utc);
        
        var doctorAvaiableAgendas = await GetDoctorAvailableAgendaByTime(doctorId, startDateTimeUtc, endDateTimeUtc, ct);

        if (doctorAvaiableAgendas.Count > 0)
        {
            Field field = new()
            {
                Name = "startDateTime",
                Value = agendaDto.StartDateTime.ToString(),
                ExMessage = "Horário livre já cadastrado."
            };
            List<Field> fields = new() { field };
                
            DataValidationException.Throw("400", "Erro no registro de agenda", "Agenda já cadastrada.", fields);
        }
        
        var newAgenda = new Agenda
        {
            StartTime = startDateTimeUtc,
            EndTime = endDateTimeUtc,
            Available = true,
            DoctorId = doctorId,
            HourlyPrice = agendaDto.hourlyPrice ?? 0,
        };
        
        var res = await agendaRepository.AddAvailableSlotAsync(newAgenda, ct);

        if (!res)
        {
            NotFoundException.Throw("404", "Médico não encontrado. Confira o id informado.");
        }

        return res;
    }
    
    public async Task<bool> UpdateAgenda(string agendaId, AgendaDto updateAgendaDto, string userRole, string userId, CancellationToken ct = default)
    {
        var agenda = await agendaRepository.GetAgendaById(agendaId, ct);
        
        if (agenda == null)
        {
            NotFoundException.Throw("404", "Agenda não encontrada.");
        }

        if (userRole == EProfile.Doctor.ToString() && userId != agenda!.DoctorId)
        {
            ForbiddenException.Throw("403", "Forbidden");
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
            DoctorId = agenda!.DoctorId,
            HourlyPrice = updateAgendaDto.hourlyPrice ?? agenda!.HourlyPrice,
        };
        
        var res = await agendaRepository.EditSlotAsync(newAgenda, ct);
        
        return res;
    }
    public async Task<List<DoctorAgendaDto>> GetDoctorAvailableAgendaByTime(string doctorId, DateTime startQueryDate, DateTime endQueryDate, CancellationToken ct = default)
    {
        return await agendaRepository.GetAvailableSlotsAsync(doctorId, startQueryDate, endQueryDate, ct);
    }
}