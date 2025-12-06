using Application.Common.Enums;
using Application.Common.Models;
using Application.Repositories;
using Common.Resources;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System.Reflection.PortableExecutable;

namespace Application.Features.Vaccinations.Commands.CreateVaccination;

public class CreateVaccinationCommandHandler(IPersonRepository personRepository, IVaccineRepository vaccineRepository, IVaccinationRepository vaccinationRepository) : IRequestHandler<CreateVaccinationCommand, Result<CreateVaccinationResponse>>
{
    public async Task<Result<CreateVaccinationResponse>> Handle(CreateVaccinationCommand request, CancellationToken cancellationToken)
    {

        // 1 - Buscar e validar a pessoa
        var personExists = await personRepository.ExistsByIdAsync(request.PersonId);
        if(!personExists)
            return Result.Failure<CreateVaccinationResponse>(Messages.PersonNotFound, ResultStatus.NotFound);

        // 2 - Buscar e validar a vacina
        var vaccine = await vaccineRepository.GetByIdAsync(request.VaccineId);
        if(vaccine is null)
            return Result.Failure<CreateVaccinationResponse>(Messages.VaccineNotFound, ResultStatus.NotFound);

        Vaccination vaccination = request.ToEntity();

        // 3 - Validar se a pessoa pode tomar aquela dose da vacina
        var validationResult = await ValidateDoseAsync(vaccine, request);

        if (validationResult.IsFailure) return validationResult.FailureAs<CreateVaccinationResponse>();

        // 4 - Cadastra a vacinação


        await vaccinationRepository.AddAsync(vaccination);
        await vaccinationRepository.SaveChangesAsync();

        return new CreateVaccinationResponse(vaccination);
    }

    public async Task<Result> ValidateDoseAsync(Vaccine vaccine, CreateVaccinationCommand request)
    {
        ArgumentNullException.ThrowIfNull(vaccine);

        ArgumentNullException.ThrowIfNull(request);

        var candidateDose = request.GetDose();

        // 1. Verificar se a dose é permitida para essa vacina
        if (vaccine.NotAllowsDose(candidateDose))
            return Result.Failure(Messages.VaccinationDoseNotAllowedForVaccine, ResultStatus.Validation);

        // 2. Buscar última vacinação
        var previousVaccination = await vaccinationRepository
            .GetLastVaccinationByPersonAndVaccine(request.PersonId, request.VaccineId);

        // 3. Se nunca tomou essa vacina
        if (previousVaccination is null)
        {
            if (candidateDose.IsNotFirstDose())
                return Result.Failure(Messages.VaccinationDoseMustBeFirst, ResultStatus.Validation);

            return Result.Success();
        }

        // 4. Verificar se a dose solicitada é a próxima correta
        var expectedNextDose = vaccine.NextDose(previousVaccination.Dose);

        if (expectedNextDose != candidateDose)
            return Result.Failure(Messages.VaccinationDoseIsWrong, ResultStatus.Validation);

        return Result.Success();
    }



}
