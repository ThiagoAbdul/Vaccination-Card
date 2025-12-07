namespace Domain.UnitTests.Entities;

using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Xunit;
using Xunit.Abstractions;

public class VaccinationDoseCompareTests
{
    private ITestOutputHelper _output;
    public VaccinationDoseCompareTests(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public void Sould_Ordenation_IsValid()
    {
        var first = new VaccinationDose { Type = VaccineDoseType.Primary, DoseNumber = 1 };
        var second = new VaccinationDose { Type = VaccineDoseType.Primary, DoseNumber = 2 };
        var third = new VaccinationDose { Type = VaccineDoseType.Primary, DoseNumber = 3 };
        var fourth = new VaccinationDose { Type = VaccineDoseType.Booster, DoseNumber = 1 };
        var fifth = new VaccinationDose { Type = VaccineDoseType.Booster, DoseNumber = 2 };

        for (int i = 0; i < 10; i++)
        {
            HashSet<VaccinationDose> vaccinations = [second, fifth, third, fourth, first];

            List<VaccinationDose> sortedVaccinations = vaccinations.Order().ToList();

            _output.WriteLine(sortedVaccinations.Count().ToString());

            sortedVaccinations.ForEach(vaccination =>
            {
                _output.WriteLine($"{vaccination.Type.ToString()} {vaccination.DoseNumber}");
            });


            Assert.Equal(sortedVaccinations[0], first);
            Assert.Equal(sortedVaccinations[1], second);
            Assert.Equal(sortedVaccinations[2], third);
            Assert.Equal(sortedVaccinations[3], fourth);
            Assert.Equal(sortedVaccinations[4], fifth);

        }


    }


    [Fact]
    public void CompareTo_BoosterVsPrimary_ReturnsPositive()
    {
        // Arrange
        var booster = new VaccinationDose
        {
            Type = VaccineDoseType.Booster,
            DoseNumber = 1
        };

        var primary = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = 1
        };

        // Act
        var result = booster.CompareTo(primary);

        // Assert
        Assert.True(result > 0);
    }


    [Fact]
    public void CompareTo_SameDoseType_CompareDoseNumber()
    {
        // Arrange
        var v1 = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = 1
        };

        var v2 = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = 3
        };

        // Act
        var result = v1.CompareTo(v2);

        // Assert
        Assert.True(result < 0); // 1 < 3
    }

    [Fact]
    public void CompareTo_SameDoseType_EqualDoseNumber_ReturnsZero()
    {
        // Arrange
        var v1 = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = 2
        };

        var v2 = new VaccinationDose
        {
            Type = VaccineDoseType.Primary,
            DoseNumber = 2
        };

        // Act
        var result = v1.CompareTo(v2);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_Booster_SameDoseType_CompareDoseNumber()
    {
        // Arrange
        var v1 = new VaccinationDose
        {
            Type = VaccineDoseType.Booster,
            DoseNumber = 2
        };

        var v2 = new VaccinationDose
        {
            Type = VaccineDoseType.Booster,
            DoseNumber = 1
        };

        // Act
        var result = v1.CompareTo(v2);

        // Assert
        Assert.True(result > 0); // 2 > 1
    }


}