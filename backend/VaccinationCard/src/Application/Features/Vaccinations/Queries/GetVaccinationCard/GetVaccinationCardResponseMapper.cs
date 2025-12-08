using Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Features.Vaccinations.Queries.GetVaccinationCard;

public class GetVaccinationCardResponseMapper
{
    public static GetVaccinationCardResponse Map(List<Vaccine> vaccines, Person person)
    {
        var orderedVaccines = vaccines.OrderBy(v => v.Name).ToList();

        var vaccinesResponse = orderedVaccines
            .Select(v => new GetVaccinationCardResponse.VaccineDetails
            {
                Id = v.Id,
                Name = v.Name
            })
            .ToList();

        var allDoses = orderedVaccines
            .SelectMany(v => v.GetDoses())
            .Distinct()
            .Order();

        var doseResponses = allDoses
            .Select(dose => new GetVaccinationCardResponse.VaccineDoseDetails
            {
                DoseNumber = dose.DoseNumber,
                Type = dose.Type,
                Vaccinations = orderedVaccines
                    .Select(v => {
                        var vaccination = v.Vaccinations.FirstOrDefault(vc => vc.Dose == dose);

                        return new GetVaccinationCardResponse.VaccinationDetails
                        {
                            Id = vaccination?.Id,
                            Applied = vaccination is not null,
                            VaccinationDate = vaccination?.VaccinationDate,
                            Available = v.AllowsDose(dose)
                        };
                    })
                    .ToList()
            })
            .ToList();

        return new GetVaccinationCardResponse
        {
            Vaccines = vaccinesResponse,
            Doses = doseResponses,
            Person = new GetVaccinationCardResponse.PersonDetails
            {
                Name = person.Name,
                BirthDate = person.BirthDate,
                Gender = person.Gender
            }
        };
    }

}
