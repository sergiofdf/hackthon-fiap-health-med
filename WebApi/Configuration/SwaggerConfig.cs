using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Domain.Enums;
using Microsoft.OpenApi.Any;

namespace Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {
                // Configuração do título e descrição
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Agenda Médica",
                    Description = "Esta API fornece serviços para agendamentos médicos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Suporte Hackathon",
                        Email = "suporte@hackathon.com.br"
                    }
                });
                
                // Configura o Swagger para exibir enums como string
                options.MapType<Specialties>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(typeof(Specialties))
                        .Select(name => new OpenApiString(name))
                        .Cast<IOpenApiAny>()
                        .ToList()
                });
                
                options.MapType<AppointmentStatus>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(typeof(AppointmentStatus))
                        .Select(name => new OpenApiString(name))
                        .Cast<IOpenApiAny>()
                        .ToList()
                });


                // Para incluir comentários XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                //Incluir exemplos no swagger
                options.ExampleFilters();

                //Configs para autenticação JWT
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Autenticacao baseada em Json Web Token. Informar somente o token, sem a palavra 'Bearer'."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            services.AddSwaggerExamplesFromAssemblyOf<Program>();

            return services;
        }
    }
}