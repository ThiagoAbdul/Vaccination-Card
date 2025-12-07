using Domain.Entities;

namespace Application.Features.Vaccines.GetAllVaccines;

public class VaccineResponse(Vaccine vaccine)
{
    public Guid Id { get; set; } = vaccine.Id;
    public string Name { get; set; } = vaccine.Name;
    public int Doses { get; set; } = vaccine.Doses;
    public int BoosterDoses { get; set; } = vaccine.BoosterDoses;
}
