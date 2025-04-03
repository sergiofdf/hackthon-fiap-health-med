using Application.Models;
using MassTransit;
using WebApi.Queues.Messages;

namespace WebApi.Queues.Publishers;

public class AddAppointmentSchedulePublisher : IAddAppointmentSchedulePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly ILogger<AddAppointmentSchedulePublisher> _logger;

    public AddAppointmentSchedulePublisher(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider, ILogger<AddAppointmentSchedulePublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _sendEndpointProvider = sendEndpointProvider;
        _logger = logger;
    }


    public async Task PublishMessage(AppointmentDto appointmentDto, CancellationToken cancellationToken = default)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:add-appointment"));

        var message =
            new SchedulingAppointmentMessage(
                appointmentDto.StartTime, 
                appointmentDto.EndTime, 
                appointmentDto.DoctorId, 
                appointmentDto.PatientId,
                "Enviada nova solicitação de agendamento de consulta."
                );
        
        await endpoint.Send(message, cancellationToken);
        _logger.LogInformation(@"Enviada nova solicitacao de agendamento de consulta. 
Médico: {@medico}. 
Paciente: {@paciente}", 
            appointmentDto.DoctorId, 
            appointmentDto.PatientId);
    }
}