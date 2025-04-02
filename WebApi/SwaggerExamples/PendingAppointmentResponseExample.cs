using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

public class PendingAppointmentResponseExample : IExamplesProvider<Appointment>
{
    public Appointment GetExamples()
    {
        return new Appointment
        {
            CreatedAt = new DateTime(2025,03,30, 08, 00, 00),
            UpdatedAt = new DateTime(2025,03,30, 09, 00, 00),
            AppointmentId = "bbc500e0-9569-4f8c-b1b9-8b717013d040",
            StartTime = new DateTime(2025,04,01, 08, 00, 00),
            EndTime = new DateTime(2025,04,01, 09, 00, 00),
            Status = AppointmentStatus.Pending,
            DoctorId = "275f38f0-1613-472d-8294-30f4b532c94b",
            PatientId = "90bb23b6-10dd-4054-9a3c-9a2660743a21",
        };
    }
}
