namespace WebApi.Queues.Messages;

public record SchedulingAppointmentMessage(
    DateTime StartTime, 
    DateTime EndTime, 
    string DoctorId, 
    string PatientId,
    string Message
    );
    