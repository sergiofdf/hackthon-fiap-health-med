using Application.Models;

namespace WebApi.Queues.Publishers;

public interface IAddAppointmentSchedulePublisher
{
    Task PublishMessage(AppointmentDto appointmentDto, CancellationToken cancellationToken = default);
}