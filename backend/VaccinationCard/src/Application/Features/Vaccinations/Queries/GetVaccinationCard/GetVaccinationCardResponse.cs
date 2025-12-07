using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;

public class GetVaccinationCardResponse
{

    public List<VaccineDetails> Vaccines { get; set; }

    public class VaccineDetails
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public List<DoseDetails> Doses { get; set; }

    }

    public class VaccinationDetails
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? Id { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateOnly? VaccinationDate { get; set; }
        public bool Applied { get; set; }
    }

    public class DoseDetails
    {
        public bool Available { get; set; }
        public VaccineDoseType Type { get; set; }
        public int DoseNumber { get; set; }
        public VaccinationDetails Vaccination { get; set; }
    }
}
