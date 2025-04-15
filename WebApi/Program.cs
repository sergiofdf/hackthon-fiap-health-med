using System.Text;
using Application.Services.AppointmentService;
using Application.Services.AuthServices;
using Application.Services.DoctorServices;
using Application.Services.UserService;
using Domain.Interfaces;
using HealthChecks.UI.Client;
using Infra;
using Infra.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using Prometheus.SystemMetrics;
using WebApi.Configuration;
using WebApi.Middlewares;
using WebApi.Queues.Publishers;

var builder = WebApplication.CreateBuilder(args);

//Config json 
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

//Config auth JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) 
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true, 
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
        };
    });

// Banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>()
    .AddSingleton<IMemoryCache, MemoryCache>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<IDoctorRepository, DoctorRepository>()
    .AddScoped<IDoctorService, DoctorService>()
    .AddScoped<IAppointmentRepository, AppointmentRepository>()
    .AddScoped<IAppointmentService, AppointmentService>()
    .AddScoped<IAgendaRepository, AgendaRepository>()
    .AddScoped<IAgendaService, AgendaService>()
    .AddScoped<IAddAppointmentSchedulePublisher, AddAppointmentSchedulePublisher>()
    .AddScoped<IUpdateAppointmentStatusPublisher, UpdateAppointmentStatusPublisher>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig(builder.Configuration);
builder.AddMassTransitConfig();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("PostgresConnection")!)
    .ForwardToPrometheus();

builder.Services.AddSystemMetrics();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponseNoExceptionDetails
});

app.UseHttpMetrics();

//app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapMetrics();

app.MapControllers();

app.Run();

public partial class Program { }