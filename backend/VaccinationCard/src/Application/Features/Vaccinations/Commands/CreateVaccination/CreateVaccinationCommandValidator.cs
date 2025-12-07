using Common.Resources;
using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Features.Vaccinations.Commands.CreateVaccination;

public class CreateVaccinationCommandValidator : AbstractValidator<CreateVaccinationCommand>
{
    public CreateVaccinationCommandValidator()
    {
        RuleFor(x => x.PersonId)
            .NotNull().WithMessage(string.Format(Messages.FieldIsMandary, nameof(Vaccination.PersonId))); // Erro do front => mensagem mais descritiva
        RuleFor(x => x.VaccineId)
            .NotNull().WithMessage(string.Format(Messages.FieldIsMandary, nameof(Vaccination.VaccineId))); // Erro do front => mensagem mais descritiva
        RuleFor(x => x.VaccinationDate)
            .NotNull().WithMessage(string.Format(Messages.FieldIsMandary, "Data de vascinação"))
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage(Messages.VaccinationCannotBeInFuture);
        RuleFor(x => x.DoseType)
            .IsInEnum().WithMessage(string.Format(Messages.FieldIsMandary, "Tipo da dose")); // Erro do front => mensagem mais descritiva

        RuleFor(x => x.DoseNumber)
            .GreaterThan(0).WithMessage(Messages.VaccinationDoseMustBeGreatherThanZero);

    }
}
