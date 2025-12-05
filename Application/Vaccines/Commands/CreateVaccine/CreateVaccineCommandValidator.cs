using Common.Resources;
using FluentValidation;

namespace Application.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommandValidator : AbstractValidator<CreateVaccineCommand>
{

    public CreateVaccineCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Messages.VaccineNameIsRequired)
            .MaximumLength(100).WithMessage(Messages.VaccineNameIsTooLong);
    }
}
