using System.Globalization;
using Application.Models;
using Application.Services.DoctorServices;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.Services.AppointmentService;

public class AppointmentService(
    IAppointmentRepository appointmentRepository,
    ILogger<AppointmentService> logger,
    IAgendaService agendaService)
    : IAppointmentService
{
    public async Task<List<Appointment>> GetPendingConfirmationAppointsAsync(string doctorId, CancellationToken cancellationToken = default)
    {
        return await appointmentRepository.GetPendingAppointmentsByDoctorIdAsync(doctorId, cancellationToken);
    }

    public async Task<Appointment> AddAppointmentAsync(AppointmentDto appointmentDto, CancellationToken cancellationToken = default)
    {
        appointmentDto.StartTime = DateTime.SpecifyKind(appointmentDto.StartTime, DateTimeKind.Utc);
        appointmentDto.EndTime = DateTime.SpecifyKind(appointmentDto.EndTime, DateTimeKind.Utc);
        
        var agendaDoctor = await agendaService.GetDoctorAvailableAgendaByTime(appointmentDto.DoctorId, appointmentDto.StartTime, appointmentDto.EndTime, cancellationToken);

        if (agendaDoctor.Count == 0)
        {
            logger.LogError("Agenda indisponível para este horário.");
            
            Field field = new()
            {
                Name = "startTime",
                Value = appointmentDto.StartTime.ToString(CultureInfo.InvariantCulture),
                ExMessage = "Agenda indisponível neste horário."
            };
            List<Field> fields = new() { field };
                
            DataValidationException.Throw("400", "Erro no agendamento de consulta.", "Agenda indisponível.", fields);
        }
        
        var appointment = new Appointment
        {
            StartTime = appointmentDto.StartTime,
            EndTime = appointmentDto.EndTime,
            Status = AppointmentStatus.Pending,
            CancellingJustification = null,
            DoctorId = appointmentDto.DoctorId,
            PatientId = appointmentDto.PatientId,
        };
        
        var res = await appointmentRepository.AddAppointmentAsync(appointment, cancellationToken);
        if (!res)
        {
            ServerException.Throw("500", "Erro no agendamento de consulta.");
        }
        
        return appointment;
    }

    public async Task<AppointmentResponseDto> UpdateAppointmentConfirmationAsync(UpdateAppointmentDto appointmentDto, CancellationToken cancellationToken = default)
    {
       var updatedAppointment = await appointmentRepository.UpdateAppointmentConfirmationAsync(appointmentDto.AppointmentId, appointmentDto.Status, appointmentDto.Reason, cancellationToken);

       if (updatedAppointment == null)
       {
           logger.LogError("Erro na atualização da consulta {@appointmentId}", appointmentDto.AppointmentId);
           ServerException.Throw("500", "Erro na atualização da consulta.");
       }

       var res = new AppointmentResponseDto
       {
           StartTime = updatedAppointment!.StartTime,
           EndTime = updatedAppointment.EndTime,
           DoctorId = updatedAppointment.DoctorId,
           PatientId = updatedAppointment.PatientId,
           Status = updatedAppointment.Status
       };
       
       return res;
    }
}