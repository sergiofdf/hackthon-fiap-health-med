using Application.Models;
using Application.Services.AppointmentService;
using Application.Services.EmailService;
using Domain.Enums;
using Infra;
using MassTransit;
using WebApi.Queues.Messages;

namespace WebApi.Queues.Consumers;

public class UpdateAppointmentStatusConsumer : IConsumer<UpdateAppointmentStatusMessage>
{
    private readonly ILogger<UpdateAppointmentStatusConsumer> _logger;
    private readonly IAppointmentService _appointmentService;
    private readonly IEmailService _emailService;
    
    public UpdateAppointmentStatusConsumer(ILogger<UpdateAppointmentStatusConsumer> logger, IAppointmentService appointmentService, IEmailService emailService)
    {
        _logger = logger;
        _appointmentService = appointmentService;
        _emailService = emailService;
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

            if (res.Status == AppointmentStatus.ApprovedByDoctor)
            {
               await _emailService.SendEmailAsync("RM357298@fiap.com.br", "Consulta Aprovada",
                    $"Sua consulta foi aprovada para o dia {res.StartTime:dd/MM/yyyy HH:mm}.");
            }

            if (res.Status == AppointmentStatus.RejectedByDoctor)
            {
                await _emailService.SendEmailAsync("RM357298@fiap.com.br", "Consulta Recusada",
                    $"Sua consulta foi recusada para o dia {res.StartTime:dd/MM/yyyy HH:mm}.");
            }

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