using Application.Services.DoctorServices;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Moq;

namespace Tests;

public class DoctorServiceTests
{
    private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
    private readonly DoctorService _service;

    public DoctorServiceTests()
    {
        _doctorRepositoryMock = new Mock<IDoctorRepository>();
        _service = new DoctorService(_doctorRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDoctorDtos()
    {
        // Arrange
        var doctors = new List<Doctor>
        {
            new()
            {
                Id = "9aadfe44-d5f6-49da-821d-c7651cc3e522",
                Name = "Tropeço",
                LastName = "Alves",
                Email = "tropeco@example.com",
                Crm = "CRM001",
                Specialty = Specialties.Neurologia
            },
            new()
            {
                Id = "5edd65ab-7a60-4b20-a129-dd32b3b233d2",
                Name = "Douglas",
                LastName = "Jones",
                Email = "douglas@example.com",
                Crm = "CRM002",
                Specialty = Specialties.Ginecologia
            }
        };

        _doctorRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctors);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Tropeço", result[0].Name);
        Assert.Equal("Douglas", result[1].Name);
    }

    [Fact]
    public async Task GetBySpecialtyAsync_ReturnsFilteredDoctorDtos()
    {
        // Arrange
        var specialty = Specialties.Geral;

        var doctors = new List<Doctor>
        {
            new Doctor
            {
                Id = "3",
                Name = "Carol",
                LastName = "Taylor",
                Email = "carol@example.com",
                Crm = "CRM003",
                Specialty = Specialties.Geral
            }
        };

        _doctorRepositoryMock
            .Setup(r => r.GetBySpecialityAsync(specialty, It.IsAny<CancellationToken>()))
            .ReturnsAsync(doctors);

        // Act
        var result = await _service.GetBySpecialtyAsync(specialty);

        // Assert
        Assert.Single(result);
        Assert.Equal("Carol", result[0].Name);
        Assert.Equal(Specialties.Geral, result[0].Specialty);
    }
}