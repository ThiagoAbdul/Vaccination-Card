using Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;
using Domain.Entities;

namespace Application.Features.Vaccinations.Queries.GetVaccinationCard;

public class GetVaccinationCardResponseMapper
{
    public static GetVaccinationCardResponse Map(IEnumerable<Vaccine> vaccines)
    {
        var doses = vaccines.SelectMany(v => v.Vaccinations).Select(vc => vc.Dose).Distinct().ToList();
        var vaccinesResponse = vaccines
            .Select(vaccine =>
            {
                var dosesResponses = doses.Select(dose =>
                {
                    var vaccination = vaccine.Vaccinations.FirstOrDefault(vc => vc.Dose == dose);

                    var vaccinationResponse =  new GetVaccinationCardResponse.VaccinationResponse
                    {
                        Id = vaccination?.Id,
                        Applied = vaccination is not null,
                        VaccinationDate = vaccination?.VaccinationDate
                    };

                    return new GetVaccinationCardResponse.DoseResponse
                    {
                        Type = dose.Type,
                        DoseNumber = dose.DoseNumber,
                        Vaccination = vaccinationResponse,
                        Available = vaccine.AllowsDose(dose),
                    };

                }).ToList();

                return new GetVaccinationCardResponse.VaccineResponse
                {
                    Id = vaccine.Id,
                    Doses = dosesResponses,
                    Name = vaccine.Name,
                };

            });

        return new GetVaccinationCardResponse
        {
            Vaccines = vaccinesResponse.ToList(),
        };


    }
}
