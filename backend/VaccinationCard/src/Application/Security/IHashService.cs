namespace Application.Security;

public interface IHashService
{
    bool Compare(string input, string hash);
    string GenerateDeterministicHash(string input);
}
