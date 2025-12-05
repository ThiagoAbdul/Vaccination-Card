namespace Application.Common.Interfaces;

public interface IMessageBus
{
    Task PublishAsync<T>(string topic, T message);
    Task PublishRangeAsync<T>(string topic, IEnumerable<T> messages);
}
