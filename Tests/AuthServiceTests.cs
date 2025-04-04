using Application.Models;
using Application.Services.AuthServices;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Fixtures;

namespace Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockConfiguration = new Mock<IConfiguration>();
        Mock<ILogger<AuthService>> mockILogger = new();
        _authService = new AuthService(_mockUserRepository.Object, _mockConfiguration.Object, mockILogger.Object);
    }

    [Fact(DisplayName = "Deve registrar um novo usuário")]
    [Trait("Metodo", "RegisterUserAsync")]
    public async Task RegisterUserAsync_ShouldReturnTrue_WhenUserRegisteredSuccessfully()
    {
        // Arrange
        var userDto = UsersFixture.CreateFakeUserDto();
        
        _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>(), default)).ReturnsAsync(true);
    
        // Act
        var result = await _authService.RegisterUserAsync(userDto);
    
        //Assert
        result.Should().BeTrue();
        _mockUserRepository.Verify(repo => repo.AddAsync(It.IsAny<User>(), default), Times.Once);
    }
    
    [Fact(DisplayName = "Deve retornar erro 400 se usuario ja existe")]
    [Trait("Metodo", "RegisterUserAsync")]
    public async Task RegisterUserAsync_ShouldReturnError400_WhenUserAlreadyRegistered()
    {
        // Arrange
        var userDto = UsersFixture.CreateFakeUserDto();
        var user = UsersFixture.CreateFakeUser();
        
        // Simula que o email já está registrado
        _mockUserRepository.Setup(repo => repo.GetByEmailAsync(userDto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _authService.RegisterUserAsync(userDto);

        // Assert
        var exceptionAssertion = await act.Should()
            .ThrowAsync<DataValidationException>();

        exceptionAssertion.Which.Code.Should().Be("400");
        exceptionAssertion.Which.ExMessage.Should().Be("Erro no registro de usuario");
        exceptionAssertion.Which.Details.Should().Contain("Email duplicado");

        exceptionAssertion.Which.Fields.Should().ContainSingle()
            .Which.Name.Should().Be("email");
    }
    
    [Fact(DisplayName = "Deve realizar login por CRM")]
    [Trait("Metodo", "LoginAsync")]
    public async Task LoginAsync_ShouldReturnToken_WhenValidCRMReceived()
    {
        // Arrange
        var loginCrm = new UserLoginDto("65214-SP", "Abc102030!");
        var user = UsersFixture.CreateFakeUser();
        
        user.PasswordHash = "AQAAAAIAAYagAAAAEGXN/xGoWl6iI4xXBgQI46Jkwo8SQIXRoLhac2UORDpnKN85HjbX2SVLdtGlC0cCuA==";
        
        _mockUserRepository.Setup(repo => repo.GetByCrmAsync(loginCrm.Login, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("chave-super-ultra-mega-aaaaaaaaaaa-secreta");
        
        var mockExpirationSection = new Mock<IConfigurationSection>();
        mockExpirationSection.Setup(x => x.Value).Returns("30");
        _mockConfiguration.Setup(config => config.GetSection("Jwt:ExpirationInMinutes"))
            .Returns(mockExpirationSection.Object);

        _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("issuer");
        _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("audience");
            
        // Act
        var result = await _authService.LoginAsync(loginCrm);
    
        //Assert
        result.Should().BeOfType<string>();
        result.Should().MatchRegex(@"^[\w-]+\.[\w-]+\.[\w-]+$");
        _mockUserRepository.Verify(repo => repo.GetByCrmAsync(loginCrm.Login, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = "Deve realizar login por Cpf")]
    [Trait("Metodo", "LoginAsync")]
    public async Task LoginAsync_ShouldReturnToken_WhenValidCpfReceived()
    {
        // Arrange
        var loginCpf = new UserLoginDto("39585865412", "Abc102030!");
        var user = UsersFixture.CreateFakeUser();
        
        user.PasswordHash = "AQAAAAIAAYagAAAAEGXN/xGoWl6iI4xXBgQI46Jkwo8SQIXRoLhac2UORDpnKN85HjbX2SVLdtGlC0cCuA==";
        
        _mockUserRepository.Setup(repo => repo.GetByCpfAsync(loginCpf.Login, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("chave-super-ultra-mega-aaaaaaaaaaa-secreta");
       
        var mockExpirationSection = new Mock<IConfigurationSection>();
        mockExpirationSection.Setup(x => x.Value).Returns("30");
        _mockConfiguration.Setup(config => config.GetSection("Jwt:ExpirationInMinutes"))
            .Returns(mockExpirationSection.Object);

        _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("issuer");
        _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("audience");
            
        // Act
        var result = await _authService.LoginAsync(loginCpf);
    
        //Assert
        result.Should().BeOfType<string>();
        result.Should().MatchRegex(@"^[\w-]+\.[\w-]+\.[\w-]+$");
        _mockUserRepository.Verify(repo => repo.GetByCpfAsync(loginCpf.Login, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = "Deve realizar login por email")]
    [Trait("Metodo", "LoginAsync")]
    public async Task LoginAsync_ShouldReturnToken_WhenValidEmailReceived()
    {
        // Arrange
        var loginEmail = new UserLoginDto("teste@email.com", "Abc102030!");
        var user = UsersFixture.CreateFakeUser();
        
        user.PasswordHash = "AQAAAAIAAYagAAAAEGXN/xGoWl6iI4xXBgQI46Jkwo8SQIXRoLhac2UORDpnKN85HjbX2SVLdtGlC0cCuA==";
        
        _mockUserRepository.Setup(repo => repo.GetByEmailAsync(loginEmail.Login, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("chave-super-ultra-mega-aaaaaaaaaaa-secreta");
       
        var mockExpirationSection = new Mock<IConfigurationSection>();
        mockExpirationSection.Setup(x => x.Value).Returns("30");
        _mockConfiguration.Setup(config => config.GetSection("Jwt:ExpirationInMinutes"))
            .Returns(mockExpirationSection.Object);

        _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("issuer");
        _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("audience");
            
        // Act
        var result = await _authService.LoginAsync(loginEmail);
    
        //Assert
        result.Should().BeOfType<string>();
        result.Should().MatchRegex(@"^[\w-]+\.[\w-]+\.[\w-]+$");
        _mockUserRepository.Verify(repo => repo.GetByEmailAsync(loginEmail.Login, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = "Deve retornar nulo caso nao tenha um login valido")]
    [Trait("Metodo", "LoginAsync")]
    public async Task LoginAsync_ShouldReturnNull_WhenInvalidLoginReceived()
    {
        // Arrange
        var loginInvalido = new UserLoginDto("testeInvalido", "Abc102030!");
        
        // Act
        var result = await _authService.LoginAsync(loginInvalido);
    
        //Assert
        result.Should().BeNull();
    }
}