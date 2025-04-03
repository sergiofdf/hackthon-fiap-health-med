using Application.Models;
using Application.Services.AppointmentService;
using Infra;
using MassTransit;
using WebApi.Queues.Messages;

namespace WebApi.Queues;

public class AddAppointmentSchedulingConsumer : IConsumer<SchedulingAppointmentMessage>
{
    private readonly ILogger<AddAppointmentSchedulingConsumer> _logger;
    private readonly IAppointmentService _appointmentService;

    public AddAppointmentSchedulingConsumer(ILogger<AddAppointmentSchedulingConsumer> logger, IAppointmentService appointmentService)
    {
        _logger = logger;
        _appointmentService = appointmentService;
    }

    public async Task Consume(ConsumeContext<SchedulingAppointmentMessage> context)
    {
        try
        {
            var appointmentDto = new AppointmentDto
            {
                StartTime = context.Message.StartTime,
                EndTime = context.Message.EndTime,
                DoctorId = context.Message.DoctorId,
                PatientId = context.Message.PatientId,
            };
            
            var res = await _appointmentService.AddAppointmentAsync(appointmentDto);
            
            _logger.LogInformation("Solicitação de agendamento de consulta {@id} registrada.", res.AppointmentId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return;
        }
    }
}