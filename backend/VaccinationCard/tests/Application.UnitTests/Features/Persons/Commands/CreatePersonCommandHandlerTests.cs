using Application.Common.Enums;
using Application.Features.People.Commands.CreatePerson;
using Application.Repositories;
using Application.Security;
using Common.Resources;
using Domain.Entities;
using Domain.ValueObjects;
using Moq;

namespace Application.UnitTests.Features.Persons.Commands;

public class CreatePersonCommandHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepoMock = new();
    private readonly Mock<IHashService> _hashServiceMock = new();
    private readonly CreatePersonCommandHandler _commandHandler;

    public CreatePersonCommandHandlerTests()
    {
        _commandHandler = new CreatePersonCommandHandler(
            _personRepoMock.Object,
            _hashServiceMock.Object
        );


    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCPFAlreadyExists()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = new Name { FirstName = "Ana", LastName = "Silva" },
            CPF = "123",
            RG = "999"
        };

        _hashServiceMock
            .Setup(x => x.GenerateDeterministicHash(It.IsAny<string>()))
            .Returns<string>(s => $"HASH_{s}");

        _personRepoMock
            .Setup(x => x.ExistsByCPFAsync("HASH_123"))
            .ReturnsAsync(true);


        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Conflict, result.Status);
        Assert.Equal(Messages.CPFAlreadyRegistered, result.Error!.Message);

        _personRepoMock.Verify(x => x.AddAsync(It.IsAny<Person>()), Times.Never);
        _personRepoMock.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreatePerson_WhenCPFDoesNotExist()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = new Name { FirstName = "Carlos", LastName = "Souza" },
            CPF = "111",
            RG = "222"
        };

        _hashServiceMock
            .Setup(x => x.GenerateDeterministicHash(It.IsAny<string>()))
            .Returns<string>(s => $"HASH_{s}");

        _personRepoMock
            .Setup(x => x.ExistsByCPFAsync("HASH_111"))
            .ReturnsAsync(false);


        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        _personRepoMock.Verify(x => x.AddAsync(It.Is<Person>(p => p.CPF == "HASH_111")), Times.Once);
        _personRepoMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldHashCPFAndRG_WhenRGIsNotNull()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = new Name { FirstName = "Joao", LastName = "Oliveira" },
            CPF = "12345",
            RG = "555"
        };

        _hashServiceMock
            .Setup(x => x.GenerateDeterministicHash(It.IsAny<string>()))
            .Returns<string>(s => $"HASH_{s}");

        _personRepoMock.Setup(x => x.ExistsByCPFAsync(It.IsAny<string>())).ReturnsAsync(false);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        _hashServiceMock.Verify(x => x.GenerateDeterministicHash("12345"), Times.Once);
        _hashServiceMock.Verify(x => x.GenerateDeterministicHash("555"), Times.Once);
    }


    [Fact]
    public async Task Handle_ShouldReturnResponseWithCorrectData()
    {
        // Arrange
        var command = new CreatePersonCommand
        {
            Name = new Name { FirstName = "Lucas", LastName = "Moura" },
            CPF = "111",
            RG = "222",
            BirthDate = new DateOnly(2000, 7, 7)
        };

        _hashServiceMock
            .Setup(x => x.GenerateDeterministicHash(It.IsAny<string>()))
            .Returns<string>(s => $"HASH_{s}");

        _personRepoMock
            .Setup(x => x.ExistsByCPFAsync("HASH_111"))
            .ReturnsAsync(false);

        // Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Lucas Moura", result.Value.Name.FullName);
        Assert.Equal(new DateOnly(2000, 7, 7), result.Value.BirthDate);
    }



}
