using Application.Models;
using Application.Services.AppointmentService;
using Application.Services.DoctorServices;
using Domain.Dto;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
    private readonly Mock<IAgendaService> _agendaServiceMock;
    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        _agendaServiceMock = new Mock<IAgendaService>();
        Mock<ILogger<AppointmentService>> loggerMock = new Mock<ILogger<AppointmentService>>();

        _service = new AppointmentService(
            _appointmentRepositoryMock.Object,
            loggerMock.Object,
            _agendaServiceMock.Object
        );
    }

    [Fact]
    public async Task GetAppointmentsAsync_ReturnsAppointments()
    {
        // Arrange
        var doctorId = "doc123";
        var status = AppointmentStatus.Pending;
        var expectedAppointments = new List<Appointment>
        {
            new() { DoctorId = doctorId, Status = status }
        };

        _appointmentRepositoryMock
            .Setup(r => r.GetAppointmentsByDoctorIdAsync(doctorId, status, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAppointments);

        // Act
        var result = await _service.GetAppointmentsAsync(doctorId, status);

        // Assert
        Assert.Single(result);
        Assert.Equal(doctorId, result[0].DoctorId);
    }

    [Fact]
    public async Task AddAppointmentAsync_ValidAppointment_ReturnsAppointment()
    {
        // Arrange
        var appointmentDto = new AppointmentDto
        {
            DoctorId = "doc1",
            PatientId = "pat1",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(30)
        };
        
        var mockDoctorAgenda = new DoctorAgendaDto(
            Id: "doc123",
            Available: true,
            StartTime: DateTime.UtcNow,
            EndTime: DateTime.UtcNow.AddHours(1),
            Name: "Sanders",
            LastName: "Das Neves",
            Email: "sanders.dasnevese@example.com",
            Crm: "CRM123456",
            Specialty: Specialties.Geriatria,
            HourlyPrice: 250.00m
        );

        _agendaServiceMock
            .Setup(s => s.GetDoctorAvailableAgendaByTime(appointmentDto.DoctorId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([mockDoctorAgenda]);

        _appointmentRepositoryMock
            .Setup(r => r.AddAppointmentAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.AddAppointmentAsync(appointmentDto);

        // Assert
        Assert.Equal(AppointmentStatus.Pending, result.Status);
        Assert.Equal(appointmentDto.DoctorId, result.DoctorId);
    }

    [Fact]
    public async Task AddAppointmentAsync_WhenNoAgenda_ThrowsDataValidationException()
    {
        // Arrange
        var appointmentDto = new AppointmentDto
        {
            DoctorId = "doc1",
            PatientId = "pat1",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(30)
        };

        _agendaServiceMock
            .Setup(s => s.GetDoctorAvailableAgendaByTime(appointmentDto.DoctorId, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<DataValidationException>(() => _service.AddAppointmentAsync(appointmentDto));
        Assert.Equal("400", ex.Code);
    }

    [Fact]
    public async Task UpdateAppointmentConfirmationAsync_Success_ReturnsDto()
    {
        // Arrange
        var appointment = new Appointment
        {
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(30),
            DoctorId = "doc1",
            PatientId = "pat1",
            Status = AppointmentStatus.ApprovedByDoctor
        };

        var dto = new UpdateAppointmentDto
        {
            AppointmentId = "appt1",
            Status = AppointmentStatus.ApprovedByDoctor,
            Reason = "Confirmed by patient"
        };

        _appointmentRepositoryMock
            .Setup(r => r.UpdateAppointmentConfirmationAsync(dto.AppointmentId, dto.Status, dto.Reason, It.IsAny<CancellationToken>()))
            .ReturnsAsync(appointment);

        // Act
        var result = await _service.UpdateAppointmentConfirmationAsync(dto);

        // Assert
        Assert.Equal(dto.Status, result.Status);
        Assert.Equal("doc1", result.DoctorId);
    }

    [Fact]
    public async Task UpdateAppointmentConfirmationAsync_NullResult_ThrowsServerException()
    {
        // Arrange
        var dto = new UpdateAppointmentDto
        {
            AppointmentId = "appt1",
            Status = AppointmentStatus.CancelledByDoctor,
            Reason = "Patient request"
        };

        _appointmentRepositoryMock
            .Setup(r => r.UpdateAppointmentConfirmationAsync(dto.AppointmentId, dto.Status, dto.Reason, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Appointment)null!);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ServerException>(() => _service.UpdateAppointmentConfirmationAsync(dto));
        Assert.Equal("500", ex.Code);
    }
}