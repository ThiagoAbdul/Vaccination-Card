using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Helpers;

public class AgeHelper
{
    public static bool IsValidAge(DateOnly birthDate)
    {
        var today = DateTime.Today;
        int age = today.Year - birthDate.Year;
        if (birthDate > DateOnly.FromDateTime(today.AddYears(-age))) // Diferença de meses
            age--;

        return age > 0 && age < 130;
    }
}
