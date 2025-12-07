using Application.Common.Helpers;

namespace Application.UnitTests.Common.Helpers;

public class AgeHelperTests
{

    [Fact]
    public void Should_ReturnTrue_When_AgeIsValid()
    {
        // 25 anos atrás
        var birthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));

        Assert.True(AgeHelper.IsValidAge(birthDate));
    }

    [Fact]
    public void Should_ReturnFalse_When_AgeIsZero()
    {
        var birthDate = DateOnly.FromDateTime(DateTime.Today);

        Assert.False(AgeHelper.IsValidAge(birthDate));
    }

    [Fact]
    public void Should_ReturnFalse_When_BirthDateIsInTheFuture()
    {
        var birthDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        Assert.False(AgeHelper.IsValidAge(birthDate));
    }

    [Fact]
    public void Should_ReturnTrue_When_BirthDateIsTody() // Fiquei em dúvida do comportamento desse,
                                                         // mas vamos super que um recém nascido possa tomar vacina.
                                                         // Geralmente excesso de validação sem ter clareza das regras de negócio não é legal
    {
        var birthDate = DateOnly.FromDateTime(DateTime.Today);

        Assert.False(AgeHelper.IsValidAge(birthDate));
    }

    [Fact]
    public void Should_ReturnFalse_When_AgeIs130()
    {
        var birthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-130));

        Assert.False(AgeHelper.IsValidAge(birthDate));
    }

    [Fact]
    public void Should_ReturnTrue_When_AgeIs129()
    {
        var birthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-129));

        Assert.True(AgeHelper.IsValidAge(birthDate));
    }

    [Fact]
    public void Should_Handle_BirthdayNotReachedYet()
    {
        // Pessoa faz aniversário amanhã
        var today = DateTime.Today;
        var birthDate = new DateOnly(today.Year - 30, today.Month, today.Day + 1);

        Assert.True(AgeHelper.IsValidAge(birthDate));
    }

    [Fact]
    public void Should_Handle_BirthdayPassed()
    {
        // Pessoa fez aniversário ontem
        var today = DateTime.Today;
        var birthDate = new DateOnly(today.Year - 30, today.Month, today.Day - 1);

        Assert.True(AgeHelper.IsValidAge(birthDate));
    }
}
