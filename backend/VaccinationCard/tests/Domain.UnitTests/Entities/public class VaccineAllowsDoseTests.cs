using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.UnitTests.Entities;

using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Xunit;

public class VaccineAllowsDoseTests
{
    [Fact]
    public void AllowsDose_PrimaryDose_WithinRange_ReturnsTrue()
    {
        // Arrange
        var vaccine = new Vaccine { Doses = 3 };
        var dose = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = 2
        };

        // Act
        var result = vaccine.AllowsDose(dose);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AllowsDose_PrimaryDose_EqualToUpperBound_ReturnsTrue()
    {
        var vaccine = new Vaccine { Doses = 2 };
        var dose = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = 2
        };

        Assert.True(vaccine.AllowsDose(dose));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AllowsDose_PrimaryDose_LessOrEqualZero_ReturnsFalse(int doseNumber)
    {
        var vaccine = new Vaccine { Doses = 3 };
        var dose = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = doseNumber
        };

        Assert.False(vaccine.AllowsDose(dose));
    }

    [Fact]
    public void AllowsDose_PrimaryDose_AboveLimit_ReturnsFalse()
    {
        var vaccine = new Vaccine { Doses = 2 };
        var dose = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = 3
        };

        Assert.False(vaccine.AllowsDose(dose));
    }

    // ---- BOOSTER TESTS ----

    [Fact]
    public void AllowsDose_BoosterDose_WithinRange_ReturnsTrue()
    {
        var vaccine = new Vaccine { BoosterDoses = 2 };
        var dose = new VaccinationDose
        {
            Type = VaccineDoseType.Booster,
            DoseNumber = 1
        };

        Assert.True(vaccine.AllowsDose(dose));
    }

    [Fact]
    public void AllowsDose_BoosterDose_EqualToUpperBound_ReturnsTrue()
    {
        var vaccine = new Vaccine { BoosterDoses = 1 };
        var dose = new VaccinationDose
        {
            Type = VaccineDoseType.Booster,
            DoseNumber = 1
        };

        Assert.True(vaccine.AllowsDose(dose));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-4)]
    public void AllowsDose_BoosterDose_LessOrEqualZero_ReturnsFalse(int doseNumber)
    {
        var vaccine = new Vaccine { BoosterDoses = 3 };
        var dose = new VaccinationDose
        {
            Type = VaccineDoseType.Booster,
            DoseNumber = doseNumber
        };

        Assert.False(vaccine.AllowsDose(dose));
    }

    [Fact]
    public void AllowsDose_BoosterDose_AboveLimit_ReturnsFalse()
    {
        var vaccine = new Vaccine { BoosterDoses = 2 };
        var dose = new VaccinationDose
        {
            Type = VaccineDoseType.Booster,
            DoseNumber = 3
        };

        Assert.False(vaccine.AllowsDose(dose));
    }
}
