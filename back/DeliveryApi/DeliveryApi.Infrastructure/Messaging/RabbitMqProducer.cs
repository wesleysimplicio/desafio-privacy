using DeliveryApi.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeliveryApi.Infrastructure.Messaging
{
    public class RabbitMqProducer : IMessageProducer
    {
        private readonly RabbitMqSettings _settings;

        public RabbitMqProducer(RabbitMqSettings settings)
        {
            _settings = settings;
        }

        public async Task SendMessageAsync<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);


            await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: _settings.QueueName,
                    body: body,
                    default
                );


            Console.WriteLine($"Mensagem enviada para a fila {_settings.QueueName}: {json}");
        }
    }
}