using Application.Models;
using MassTransit;
using WebApi.Queues.Messages;

namespace WebApi.Queues.Publishers;

public class UpdateAppointmentStatusPublisher : IUpdateAppointmentStatusPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly ILogger<UpdateAppointmentStatusPublisher> _logger;

    public UpdateAppointmentStatusPublisher(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider, ILogger<UpdateAppointmentStatusPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _sendEndpointProvider = sendEndpointProvider;
        _logger = logger;
    }
    
    public async Task PublishMessage(UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken = default)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:update-appointment"));

        var message =
            new UpdateAppointmentStatusMessage(
                updateAppointmentDto.AppointmentId,
                updateAppointmentDto.Status,
                updateAppointmentDto.Reason,
                "Nova atualização de consulta enviada."
                );
        
        await endpoint.Send(message, cancellationToken);
        
        _logger.LogInformation(@"Enviada nova solicitacao de atualização de consulta. 
Consulta: {@consulta}. 
Status: {@status}", 
            updateAppointmentDto.AppointmentId, 
            updateAppointmentDto.Status);
    }
}