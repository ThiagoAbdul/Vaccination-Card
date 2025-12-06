using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Helpers;

public class CPFHelper
{
    public static bool IsCPFValid(string cpf)
    {
        // Peguei um algoritmo da internet obviamente,
        // mas lá em 2021 eu já chegeui a fazer esse algoritmo na mão com python,
        // me senti super útil kkk não conhecia ainda o conceito de lib e reaproveitamnto de código

        if(string.IsNullOrWhiteSpace(cpf))
            return false;

        cpf = new string(cpf.Where(char.IsDigit).ToArray());
        if (cpf.Length != 11)
            return false;

        // Validação real de CPF (DV)
        int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf[..9];
        int sum = 0;

        for (int i = 0; i < 9; i++)
            sum += int.Parse(tempCpf[i].ToString()) * mult1[i];

        int remainder = sum % 11;
        int digit = remainder < 2 ? 0 : 11 - remainder;

        string cpfWithDigit = tempCpf + digit;
        sum = 0;

        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpfWithDigit[i].ToString()) * mult2[i];

        remainder = sum % 11;
        int digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cpf == cpfWithDigit + digit2;

    }
}

