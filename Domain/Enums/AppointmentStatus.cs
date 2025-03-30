namespace Domain.Enums;

public enum AppointmentStatus
{
    Pending = 1,
    ApprovedByDoctor = 2,
    RejectedByDoctor = 3,
    CancelledByDoctor = 4,
    CancelledByPatient = 5,
    CancelledByAdmin = 6
}