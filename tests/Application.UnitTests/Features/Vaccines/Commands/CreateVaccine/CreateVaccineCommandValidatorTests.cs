using Application.Features.Vaccines.Commands.CreateVaccine;
using Common.Resources;
using FluentValidation.TestHelper;


namespace Application.UnitTests.Features.Vaccines.Commands.CreateVaccine;

public class CreateVaccineCommandValidatorTests
{
    private readonly CreateVaccineCommandValidator _validator;
    public CreateVaccineCommandValidatorTests()
    {
        _validator = new CreateVaccineCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var command = new CreateVaccineCommand
        {
            Name = "",
            Doses = 1
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
              .WithErrorMessage(Messages.VaccineNameIsRequired);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Doses_Is_Zero_Or_Negative(int doses)
    {
        // Arrange
        var command = new CreateVaccineCommand
        {
            Name = "Covid",
            Doses = doses
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Doses)
              .WithErrorMessage(Messages.VaccineDosesCannotBeZero);
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Command_Is_Valid()
    {
        // Arrange
        var command = new CreateVaccineCommand
        {
            Name = "Pfizer",
            Doses = 2
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

}
