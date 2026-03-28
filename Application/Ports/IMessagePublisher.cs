namespace WarehouseStockService.Application.Ports;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default);
}
