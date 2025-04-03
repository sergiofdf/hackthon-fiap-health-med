using Application.Models;
using Application.Services.AppointmentService;
using Domain.Enums;
using Infra;
using MassTransit;
using WebApi.Queues.Messages;

namespace WebApi.Queues.Consumers;

public class UpdateAppointmentStatusConsumer : IConsumer<UpdateAppointmentStatusMessage>
{
    private readonly ILogger<UpdateAppointmentStatusConsumer> _logger;
    private readonly IAppointmentService _appointmentService;

    public UpdateAppointmentStatusConsumer(ILogger<UpdateAppointmentStatusConsumer> logger, IAppointmentService appointmentService)
    {
        _logger = logger;
        _appointmentService = appointmentService;
    }

    public async Task Consume(ConsumeContext<UpdateAppointmentStatusMessage> context)
    {
        try
        {
            var updateAppointmentDto = new UpdateAppointmentDto
            {
                AppointmentId = context.Message.AppointmentId,
                Status = context.Message.Status,
                Reason = context.Message.Reason
            };
            
            var res = await _appointmentService.UpdateAppointmentConfirmationAsync(updateAppointmentDto);

            _logger.LogInformation(@"Status consulta atualizado.
Consulta: {@consulta}
Status: {@status}", context.Message.AppointmentId, res.Status);
            
        }
        
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return;
        }
    }
}