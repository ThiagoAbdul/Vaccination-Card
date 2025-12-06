using Application.Features.People.Commands.CreatePerson;
using Common.Resources;
using Domain.Enums;
using Domain.ValueObjects;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UnitTests.Features.Persons.Commands;

public class CreatePersonCommandValidatorTests
{
    private readonly CreatePersonCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_NameIsNull() // Esse salvou
    {
        var command = new CreatePersonCommand
        {
            Name = null!
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(Messages.PersonNameIsRequired);
    }

    [Fact]
    public void Should_HaveError_When_NameIsInvalid()
    {
        var command = new CreatePersonCommand
        {
            Name = new Name { FirstName = "", LastName = "" }
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(Messages.InvalidPersonName);
    }

    [Fact]
    public void Should_HaveError_When_BirthDateIsInTheFuture()
    {
        var command = new CreatePersonCommand
        {
            Name = new Name { FirstName = "Ana", LastName = "Silva" },
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate)
            .WithErrorMessage(Messages.InvalidBirthdate);
    }

    [Fact]
    public void Should_HaveError_When_CPFIsInvalid()
    {
        var command = new CreatePersonCommand
        {
            Name = new Name { FirstName = "Ana", LastName = "Silva" },
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
            Gender = Gender.FEMALE,
            CPF = "123" // inválido
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CPF)
            .WithErrorMessage(string.Format(Messages.InvalidField, "CPF"));
    }

    [Theory]
    [InlineData("147.195.360-26")]
    [InlineData("390.950.230-01")]
    [InlineData("174.684.910-03")]
    [InlineData("53487836092")]
    public void Should_NotHaveErrors_When_CommandIsValid(string cpf)
    {
        var command = new CreatePersonCommand
        {
            Name = new Name { FirstName = "Ana", LastName = "Silva" },
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
            Gender = Gender.FEMALE,
            CPF = cpf // exemplo válido
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }



}
