using Application.Models;
using Domain.Enums;
using Swashbuckle.AspNetCore.Filters;

public class UpdateAppointmentExamples : IMultipleExamplesProvider<UpdateAppointmentDto>
{
    public IEnumerable<SwaggerExample<UpdateAppointmentDto>> GetExamples()
    {
        var aprovacaoMedico = new UpdateAppointmentDto
        {
            AppointmentId = "bbc500e0-9569-4f8c-b1b9-8b717013d040",
            Status = AppointmentStatus.ApprovedByDoctor,
            Reason = null
        };
        
        var cancelamentoMedico = new UpdateAppointmentDto
        {
            AppointmentId = "bbc500e0-9569-4f8c-b1b9-8b717013d040",
            Status = AppointmentStatus.CancelledByDoctor,
            Reason = null
        };
        
        var cancelamentoPaciente = new UpdateAppointmentDto
        {
            AppointmentId = "bbc500e0-9569-4f8c-b1b9-8b717013d040",
            Status = AppointmentStatus.CancelledByPatient,
            Reason = "Será necessário reagendamento devido a conflito com agenda pessoal."
        };
        
        yield return new SwaggerExample<UpdateAppointmentDto>()
        {
            Name = "Aprovado por médico",
            Description = "Exemplo de aprovação da consulta por médico.",
            Value = aprovacaoMedico
        };

        yield return new SwaggerExample<UpdateAppointmentDto>
        {
            Name = "Cancelado por médico",
            Description = "Exemplo de cancelamento da consulta por médico.",
            Value = cancelamentoMedico
        };
        
        yield return new SwaggerExample<UpdateAppointmentDto>
        {
            Name = "Cancelado por paciente",
            Description = "Exemplo de cancelamento da consulta pelo paciente.",
            Value = cancelamentoPaciente
        };
    }
}