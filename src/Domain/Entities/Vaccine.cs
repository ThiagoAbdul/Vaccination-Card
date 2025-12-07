using Common.Resources;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Vaccine : AuditableEntity<Guid>
{
    public string Name { get; set; }
    public int Doses { get; set; }
    public int BoosterDoses { get; set; }
    public List<Vaccination> Vaccinations { get; set; }

    public bool AllowsDose(VaccinationDose dose)
    {
        if(dose.Type == VaccineDoseType.Primary)
        {
            return dose.DoseNumber > 0 && dose.DoseNumber <= Doses;
        }

        return dose.DoseNumber > 0 && dose.DoseNumber <= BoosterDoses;
    }

    public bool NotAllowsDose(VaccinationDose dose) => !AllowsDose(dose);

    public VaccinationDose? NextDose(VaccinationDose dose)
    {
        ArgumentNullException.ThrowIfNull(dose);
        ArgumentOutOfRangeException.ThrowIfNegative(dose.DoseNumber);

        // ---- Fluxo: Doses Primárias ----
        if (dose.Type == VaccineDoseType.Primary)
        {
            // Se ainda há primárias disponíveis
            if (dose.DoseNumber < Doses)
                return new VaccinationDose(VaccineDoseType.Primary, dose.DoseNumber + 1);

            // Última primária → próxima é 1º reforço
            if (dose.DoseNumber == Doses)
            {
                if (BoosterDoses > 0)
                    return new VaccinationDose(VaccineDoseType.Booster, 1);

                return null; // Vacina sem reforço
            }

            // Invalid: excedeu o total permitido
            throw new InvalidVaccinationDoseException();
        }

        // ---- Fluxo: Doses de Reforço ----
        if (dose.Type == VaccineDoseType.Booster)
        {
            if (BoosterDoses == 0)
                throw new InvalidVaccinationDoseException();

            if (dose.DoseNumber < BoosterDoses)
                return new VaccinationDose(VaccineDoseType.Booster, dose.DoseNumber + 1);

            if (dose.DoseNumber == BoosterDoses)
                return null; // Acabaram os reforços

            throw new InvalidVaccinationDoseException();
        }

        throw new InvalidVaccinationDoseException();
    }

    public IEnumerable<VaccinationDose> GetDoses()
    {
        var doses = new VaccinationDose[Doses + BoosterDoses];


        for(int i = 0; i < Doses; i++)
        {
            doses[i] = new VaccinationDose(VaccineDoseType.Primary, i + 1);
        }

        for (int i = 0; i < BoosterDoses; i++)
        {
            doses[Doses + i] = new VaccinationDose(VaccineDoseType.Booster, i + 1);
        }

        return doses;

    }

}

public class InvalidVaccinationDoseException() : Exception(Messages.VaccinationDoseNotAllowedForVaccine) { }
