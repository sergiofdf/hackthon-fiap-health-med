using Application.Models;

namespace WebApi.Queues.Publishers;

public interface IUpdateAppointmentStatusPublisher
{
    Task PublishMessage(UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken = default);
}