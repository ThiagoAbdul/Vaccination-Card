using Application.Common.Interfaces;
using System.Text.Json;

namespace Infrastructure.Message;

public class MockMessageBus : IMessageBus
{
    public async Task PublishAsync<T>(string topic, T message)
    {
        var payload = JsonSerializer.Serialize<T>(message);
        // TODO
    }

    public async Task PublishRangeAsync<T>(string topic, IEnumerable<T> messages)
    {
        var payloads = messages.Select(x => JsonSerializer.Serialize<T>(x));

        // TODO
    }
}
