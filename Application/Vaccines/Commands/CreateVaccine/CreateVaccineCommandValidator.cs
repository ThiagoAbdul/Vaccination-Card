using FluentValidation;

namespace Application.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommandValidator : AbstractValidator<CreateVaccineCommand>
{

    public CreateVaccineCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Vaccine name is required.")
            .MaximumLength(100).WithMessage("Vaccine name must not exceed 100 characters.");
    }
}
