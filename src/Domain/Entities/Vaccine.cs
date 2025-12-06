using Common.Resources;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Vaccine : AuditableEntity<Guid>
{
    public string Name { get; set; }
    public int Doses { get; set; }
    public int BoosterDoses { get; set; }

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


}

public class InvalidVaccinationDoseException() : Exception(Messages.VaccinationDoseNotAllowedForVaccine) { }
