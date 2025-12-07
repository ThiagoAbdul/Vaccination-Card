using Application.Security;
using Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace Infrastructure.Security;

public class MockVaultClient() : IVaultClient
{
    private readonly Dictionary<string, string> _keys = new() // Vai ser singleton
    {

    };

    public Task<string> GetSecretFromVaultAsync(string keyName)
    {
        return Task.FromResult(_keys[keyName]);
    }
}
