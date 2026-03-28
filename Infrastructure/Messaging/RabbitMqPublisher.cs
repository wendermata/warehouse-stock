using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Text.Json;
using WarehouseStockService.Application.Ports;

namespace WarehouseStockService.Infrastructure.Messaging;

public sealed class RabbitMqPublisher : IMessagePublisher, IHostedService, IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqPublisher(RabbitMqSettings settings)
    {
        _settings = settings;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
            await _channel.CloseAsync(cancellationToken);
        if (_connection is not null)
            await _connection.CloseAsync(cancellationToken);
    }

    public async Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default)
    {
        if (_channel is null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");

        var body = JsonSerializer.SerializeToUtf8Bytes(message);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await _channel.BasicPublishAsync(
            exchange: _settings.ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
