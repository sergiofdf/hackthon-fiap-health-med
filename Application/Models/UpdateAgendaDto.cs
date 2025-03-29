using Domain.Enums;

namespace Application.Models;

public record UpdateAgendaDto(
    bool Available, 
    DateTime? StartDateTime, 
    DateTime? EndDateTime
    );


