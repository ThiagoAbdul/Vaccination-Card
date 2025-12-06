using Infrastructure.Security;
using Newtonsoft.Json.Linq;

namespace Infrastructure.UnitTests.Security;

public class HashServiceTests
{
    private HashService _hashService = new();

    [Theory]
    [InlineData("smallValue")]
    [InlineData("BigValueksdkdksj fdslkfkldkjfdkjffjk dfkd")]
    public void Shold_ReturnTrue_When_ValuesAreEquals(string value)
    {

        string plain = value;
        string hash = _hashService.GenerateDeterministicHash(value);

        var equals = _hashService.Compare(input: plain, hash: hash);

        Assert.True(equals);
    }

    [Theory]
    [InlineData("smallValue")]
    [InlineData("BigValueksdkdksj fdslkfkldkjfdkjffjk dfkd")]
    public void Shold_ReturnFalse_When_ValuesAreDifferents(string value)
    {
        string plain = "other value";
        string hash = _hashService.GenerateDeterministicHash(value);

        var equals = _hashService.Compare(input: plain, hash: hash);

        Assert.False(equals);
    }
}
