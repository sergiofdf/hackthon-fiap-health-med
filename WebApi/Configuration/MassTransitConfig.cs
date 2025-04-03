using MassTransit;
using WebApi.Queues;

namespace WebApi.Configuration;

public static class MassTransitConfig
{
    public static WebApplicationBuilder AddMassTransitConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            
            busConfigurator.AddConsumers(typeof(Program).Assembly);

            busConfigurator.UsingRabbitMq((ctx, cfg) =>
            {
                var host = builder.Configuration["RabbitMQConnection:Host"]!;
                var username = builder.Configuration["RabbitMQConnection:Username"]!;
                var password = builder.Configuration["RabbitMQConnection:Password"]!;

                cfg.Host(new Uri(host), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                
                cfg.ReceiveEndpoint("addappointment", e =>
                {
                    e.ConfigureConsumer<AddAppointmentSchedulingConsumer>(ctx);
                });

                
                cfg.ConfigureEndpoints(ctx);
                cfg.UseCircuitBreaker(cb =>
                {
                    cb.ActiveThreshold = 5;
                    cb.TrackingPeriod = TimeSpan.FromSeconds(10);
                    cb.ResetInterval = TimeSpan.FromMinutes(1);
                });
            });
        });
        
        return builder; 
    }
}