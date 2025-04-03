using Domain.Enums;

namespace WebApi.Queues.Messages;

public record UpdateAppointmentStatusMessage(
    string AppointmentId, 
    AppointmentStatus Status,
    string? Reason,
    string? Message
    );
    