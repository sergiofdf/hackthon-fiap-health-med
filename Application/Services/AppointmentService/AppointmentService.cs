using System.Globalization;
using Application.Models;
using Application.Services.DoctorServices;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.Extensions.Logging;

namespace Application.Services.AppointmentService;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAgendaService _agendaService;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IAppointmentRepository appointmentRepository, ILogger<AppointmentService> logger, IAgendaService agendaService)
    {
        _appointmentRepository = appointmentRepository;
        _logger = logger;
        _agendaService = agendaService;
    }
    
    public async Task<List<Appointment>> GetPendingConfirmationAppointsAsync(string doctorId)
    {
        return await _appointmentRepository.GetPendingAppointmentsByDoctorIdAsync(doctorId);
    }

    public async Task<bool> AddAppointmentAsync(AppointmentDto appointmentDto)
    {
        appointmentDto.StartTime = DateTime.SpecifyKind(appointmentDto.StartTime, DateTimeKind.Utc);
        appointmentDto.EndTime = DateTime.SpecifyKind(appointmentDto.EndTime, DateTimeKind.Utc);
        
        var agendaDoctor = await _agendaService.GetDoctorAvailableAgendaByTime(appointmentDto.DoctorId, appointmentDto.StartTime, appointmentDto.EndTime);

        if (agendaDoctor.Count == 0)
        {
            _logger.LogError("Agenda indisponível para este horário.");
            
            Field field = new()
            {
                Name = "startTime",
                Value = appointmentDto.StartTime.ToString(CultureInfo.InvariantCulture),
                ExMessage = "Agenda indisponível neste horário."
            };
            List<Field> fields = new() { field };
                
            DataValidationException.Throw("400", "Erro no agendamento de consulta.", "Agenda indisponível.", fields);
            return false;
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
        
        return await _appointmentRepository.AddAppointmentAsync(appointment);
    }

    public async Task<bool> UpdateAppointmentConfirmationAsync(string appointmentId, bool confirmed, string applicantId)
    {
        throw new NotImplementedException();
    }
}