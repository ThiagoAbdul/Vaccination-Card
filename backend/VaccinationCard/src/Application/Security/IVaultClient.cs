namespace Application.Security;

public interface IVaultClient
{
    Task<string> GetSecretFromVaultAsync(string keyName);
}
