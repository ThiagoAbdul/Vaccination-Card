using Application.Common.Helpers;
using Common.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.People.Commands.CreatePerson;

public class CreatePersonCommandValidator : AbstractValidator<CreatePersonCommand>
{
    public CreatePersonCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Messages.PersonNameIsRequired)
            .Must(x => x.IsValid()).WithMessage(Messages.InvalidPersonName);
        RuleFor(x => x.BirthDate)
            .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage(Messages.InvalidBirthdate)
            .Must(AgeHelper.IsValidAge).WithMessage(Messages.InvalidBirthdate);

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage(Messages.PersonGenderIsInvalid);

        RuleFor(x => x.CPF)
            .Must(CPFHelper.IsCPFValid).WithMessage(string.Format(Messages.InvalidField, "CPF"));

        // RuleFor() ... outros campos
    }
}
