using Application.Security;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security;

public class HashService : IHashService
{
    public string GenerateDeterministicHash(string input) // Hash determinístico
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash); 
    }

    public bool Compare(string input, string hash)
    {
        return GenerateDeterministicHash(input).Equals(hash);
    }
}
