namespace Domain.UnitTests.Entities;

using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Xunit;

public class VaccineNextDoseTests
{
    // ---- PRIMARY DOSES ----

    [Fact]
    public void NextDose_Primary_NotLast_ReturnsNextPrimary()
    {
        var vaccine = new Vaccine { Doses = 3, BoosterDoses = 2 };
        var dose = new VaccinationDose(VaccineDoseType.Primary, 2);

        var next = vaccine.NextDose(dose);

        Assert.NotNull(next);
        Assert.Equal(VaccineDoseType.Primary, next.Type);
        Assert.Equal(3, next.DoseNumber);
    }

    [Fact]
    public void NextDose_Primary_LastPrimary_ReturnsFirstBooster()
    {
        var vaccine = new Vaccine { Doses = 2, BoosterDoses = 2 };
        var dose = new VaccinationDose(VaccineDoseType.Primary, 2);

        var next = vaccine.NextDose(dose);

        Assert.NotNull(next);
        Assert.Equal(VaccineDoseType.Booster, next.Type);
        Assert.Equal(1, next.DoseNumber);
    }

    [Fact]
    public void NextDose_Primary_LastPrimary_NoBooster_ReturnsNull()
    {
        var vaccine = new Vaccine { Doses = 2, BoosterDoses = 0 };
        var dose = new VaccinationDose(VaccineDoseType.Primary, 2);

        var next = vaccine.NextDose(dose);

        Assert.Null(next);
    }

    [Fact]
    public void NextDose_Primary_ExceedsLimit_ThrowsException()
    {
        var vaccine = new Vaccine { Doses = 2 };
        var dose = new VaccinationDose(VaccineDoseType.Primary, 3);

        Assert.Throws<InvalidVaccinationDoseException>(() => vaccine.NextDose(dose));
    }

    // ---- BOOSTER ----

    [Fact]
    public void NextDose_Booster_NotLast_ReturnsNextBooster()
    {
        var vaccine = new Vaccine { BoosterDoses = 3 };
        var dose = new VaccinationDose(VaccineDoseType.Booster, 1);

        var next = vaccine.NextDose(dose);

        Assert.NotNull(next);
        Assert.Equal(VaccineDoseType.Booster, next.Type);
        Assert.Equal(2, next.DoseNumber);
    }

    [Fact]
    public void NextDose_Booster_LastBooster_ReturnsNull()
    {
        var vaccine = new Vaccine { BoosterDoses = 2 };
        var dose = new VaccinationDose(VaccineDoseType.Booster, 2);

        var next = vaccine.NextDose(dose);

        Assert.Null(next);
    }

    [Fact]
    public void NextDose_Booster_ExceedsLimit_ThrowsException()
    {
        var vaccine = new Vaccine { BoosterDoses = 2 };
        var dose = new VaccinationDose(VaccineDoseType.Booster, 3);

        Assert.Throws<InvalidVaccinationDoseException>(() => vaccine.NextDose(dose));
    }

    [Fact]
    public void NextDose_Booster_ButVaccineHasNoBoosters_ThrowsException()
    {
        var vaccine = new Vaccine { Doses = 2, BoosterDoses = 0 };
        var dose = new VaccinationDose(VaccineDoseType.Booster, 1);

        Assert.Throws<InvalidVaccinationDoseException>(() => vaccine.NextDose(dose));
    }


    [Fact]
    public void NextDose_NullDose_ThrowsArgumentNull()
    {
        var vaccine = new Vaccine { Doses = 2 };
        Assert.Throws<ArgumentNullException>(() => vaccine.NextDose(null!));
    }
}
