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
                            Available = v.AllowsDose(dose),
                            VaccineId = v.Id
                        };
                    })
                    .ToList()
            })
            .ToList();

        // Vou marcar a vacina faltante

        for(int i = 0; i < vaccines.Count; i++)
        {
            foreach(var dose in doseResponses)
            {
                var vaccination = dose.Vaccinations[i];

                if (!vaccination.Applied && vaccination.Available)
                {
                    vaccination.Absent = true;
                    break;
                }
            }
        }

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
