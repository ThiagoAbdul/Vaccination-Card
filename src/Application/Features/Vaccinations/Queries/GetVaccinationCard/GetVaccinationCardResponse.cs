using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;

public class GetVaccinationCardResponse
{

    public List<VaccineResponse> Vaccines { get; set; }

    public class VaccineResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public List<DoseResponse> Doses { get; set; }

    }

    public class VaccinationResponse
    {
        public Guid? Id { get; init; }
        public DateOnly? VaccinationDate { get; set; }
        public bool Applied { get; set; }
    }

    public class DoseResponse
    {
        public bool Available { get; set; }
        public VaccineDoseType Type { get; set; }
        public int DoseNumber { get; set; }
        public VaccinationResponse Vaccination { get; set; }
    }
}
