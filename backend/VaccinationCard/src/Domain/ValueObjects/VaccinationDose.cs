using Domain.Entities;
using Domain.Enums;

namespace Domain.ValueObjects;

public class VaccinationDose : IComparable<VaccinationDose>
{
    public VaccineDoseType Type { get; set; }

    public int DoseNumber { get; set; }

    public bool IsFirstDose()
    {
        return Type == VaccineDoseType.Primary && DoseNumber == 1;
    }

    public VaccinationDose()
    {
        
    }

    public VaccinationDose(VaccineDoseType type, int doseNumber)
    {
        Type = type;
        DoseNumber = doseNumber;
    }

    public bool IsNotFirstDose() => !IsFirstDose();


    public int CompareTo(VaccinationDose? other)
    {
        if (other is null) return 1;
        if (Type == other.Type && DoseNumber == other.DoseNumber) return 0;
        if (Type == VaccineDoseType.Booster && other.Type == VaccineDoseType.Primary) return 1;
        if (Type == VaccineDoseType.Primary && other.Type == VaccineDoseType.Booster) return -1;
        return DoseNumber.CompareTo(other.DoseNumber);
    }

    // Override nos operadores para o código ficar mais elegante
    public static bool operator ==(VaccinationDose? a, VaccinationDose? b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if (a is null || b is null)
            return false;

        return a.Type == b.Type && a.DoseNumber == b.DoseNumber;
    }

    public static bool operator !=(VaccinationDose? a, VaccinationDose? b)
        => !(a == b);

    public override bool Equals(object? obj)
    {
        return obj is VaccinationDose other &&
               Type == other.Type &&
               DoseNumber == other.DoseNumber;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, DoseNumber);
    }

}
