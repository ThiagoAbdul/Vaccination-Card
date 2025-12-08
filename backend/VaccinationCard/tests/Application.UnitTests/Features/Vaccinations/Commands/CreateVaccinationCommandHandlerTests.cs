using Application.Features.Vaccinations.Commands.CreateVaccination;
using Application.Repositories;
using Common.Resources;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Moq;

namespace Application.UnitTests.Features.Vaccinations.Commands;

public class CreateVaccinationCommandHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock = new();
    private readonly Mock<IVaccineRepository> _vaccineRepositoryMock = new();
    private readonly Mock<IVaccinationRepository> _vaccinationRepositoryMock = new();
    private readonly CreateVaccinationCommandHandler _commandHandler;

    public CreateVaccinationCommandHandlerTests()
    {
        _commandHandler = new CreateVaccinationCommandHandler(
            _personRepositoryMock.Object, 
            _vaccineRepositoryMock.Object,
            _vaccinationRepositoryMock.Object
        );
    }



    [Fact]
    public async Task ValidateDoseAsync_DoseNotAllowed_ReturnsFailure()
    {
        var vaccine = new Vaccine { Doses = 1 };
        var command = new CreateVaccinationCommand 
        {  
            DoseNumber = 2,
            DoseType = VaccineDoseType.Primary
        };


        _vaccinationRepositoryMock
            .Setup(r => r.GetLastVaccinationByPersonAndVaccine(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Vaccination?)null);

        var result = await _commandHandler.ValidateDoseAsync(vaccine, command);

        Assert.False(result.IsSuccess);
        Assert.Equal(Messages.VaccinationDoseNotAllowedForVaccine, result.Error.Message);
    }

    [Fact]
    public async Task ValidateDoseAsync_FirstVaccination_NotDoseOne_ReturnsFailure()
    {
        var vaccine = new Vaccine { Doses = 3 };

        var command = new CreateVaccinationCommand { DoseType = VaccineDoseType.Primary, DoseNumber = 2 };

        _vaccinationRepositoryMock
            .Setup(r => r.GetLastVaccinationByPersonAndVaccine(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Vaccination?)null);

        var result = await _commandHandler.ValidateDoseAsync(vaccine, command);

        Assert.False(result.IsSuccess);
        Assert.Equal(Messages.VaccinationDoseMustBeFirst, result.Error.Message);
    }

    [Fact]
    public async Task ValidateDoseAsync_FirstVaccination_DoseOne_ReturnsSuccess()
    {
        var vaccine = new Vaccine { Doses = 3 };

        var command = new CreateVaccinationCommand { DoseType = VaccineDoseType.Primary, DoseNumber = 1};

        _vaccinationRepositoryMock
            .Setup(r => r.GetLastVaccinationByPersonAndVaccine(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync((Vaccination?)null);

        var result = await _commandHandler.ValidateDoseAsync(vaccine, command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateDoseAsync_CorrectNextDose_ReturnsSuccess()
    {
        var vaccine = new Vaccine { Doses = 3, BoosterDoses = default };

        var prev = new Vaccination
        {
            Dose = new VaccinationDose(VaccineDoseType.Primary, 1)
        };

        var command = new CreateVaccinationCommand { DoseType = VaccineDoseType.Primary, DoseNumber = 2 };

        _vaccinationRepositoryMock
            .Setup(r => r.GetLastVaccinationByPersonAndVaccine(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(prev);

        var result = await _commandHandler.ValidateDoseAsync(vaccine, command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidateDoseAsync_WrongNextDose_ReturnsFailure()
    {
        var vaccine = new Vaccine { Doses = 3 };

        var prev = new Vaccination
        {
            Dose = new VaccinationDose(VaccineDoseType.Primary, 1)
        };

        var command = new CreateVaccinationCommand { DoseType = VaccineDoseType.Primary, DoseNumber = 3 };

        _vaccinationRepositoryMock
            .Setup(r => r.GetLastVaccinationByPersonAndVaccine(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(prev);

        var result = await _commandHandler.ValidateDoseAsync(vaccine, command);

        Assert.False(result.IsSuccess);
        Assert.Equal(Messages.VaccinationDoseIsWrong, result.Error.Message);
    }


    [Fact]
    public async Task ValidateDoseAsync_InvalidVaccinationDate_ReturnsFailure()
    {
        var vaccine = new Vaccine { Doses = 3 };

        var previous = new Vaccination
        {
            Dose = new VaccinationDose(VaccineDoseType.Primary, 1),
            VaccinationDate = new DateOnly(2025, 01, 10)
        };

        var command = new CreateVaccinationCommand
        {
            DoseType = VaccineDoseType.Primary,
            DoseNumber = 2,
            VaccinationDate = new DateOnly(2025, 01, 10) // <= mesma data -> deve falhar
        };

        _vaccinationRepositoryMock
            .Setup(r => r.GetLastVaccinationByPersonAndVaccine(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(previous);

        var result = await _commandHandler.ValidateDoseAsync(vaccine, command);

        Assert.False(result.IsSuccess);
        Assert.Equal(Messages.InvalidVaccinationDate, result.Error.Message);
    }



}
