using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using System;
using System.Text.Json.Serialization;

namespace Application.Features.Vaccinations.Queries.GetVaccinationCardByPersonId;

public class GetVaccinationCardResponse
{
    public PersonDetails Person { get; set; }
    public List<VaccineDetails> Vaccines { get; set; }
    public List<VaccineDoseDetails> Doses { get; set; }

    public class VaccineDetails
    {
        public Guid Id { get; init; }
        public string Name { get; init; }

    }

    public class VaccinationDetails
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Guid? Id { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateOnly? VaccinationDate { get; set; }
        public bool Applied { get; set; }
        public bool Available { get; set; }
        public bool Absent { get; set; }
        public Guid VaccineId { get; set; }

    }

    public class VaccineDoseDetails
    {
        public VaccineDoseType Type { get; set; }
        public int DoseNumber { get; set; }
        public List<VaccinationDetails> Vaccinations { get; set; }
    }

    public class PersonDetails
    {
        public Name Name { get; set; }
        public Gender Gender { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}
