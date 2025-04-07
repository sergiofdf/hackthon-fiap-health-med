using Domain.Enums;

namespace Application.Models;

public record AgendaDto(
    bool Available, 
    DateTime? StartDateTime, 
    DateTime? EndDateTime,
    decimal? hourlyPrice
    );


