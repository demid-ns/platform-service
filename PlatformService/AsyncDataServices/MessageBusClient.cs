using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<MessageBusClient> _logger;

        private IConnection _connection;
        private IModel _channel;

        public MessageBusClient(
            IConfiguration configuration,
            IMapper mapper,
            ILogger<MessageBusClient> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;

            SetupConnection();
        }

        public void PublishNewPlatform(Platform platform)
        {
            var dto = _mapper.Map<PlatformPublishedDto>(platform);
            dto.Event = "Platform_Published";
            var message = JsonSerializer.Serialize(dto);

            if (_connection.IsOpen)
            {
                _logger.LogInformation($"--> RabbitMQ connection open, sending message...");
                SendMessage(message);
            }
            else
            {
                _logger.LogInformation($"--> RabbitMQ connection closed, not sending");
            }
        }

        private void SetupConnection()
        {
            string hostName = _configuration["RabbitMqHost"];
            int port = int.Parse(_configuration["RabbitMqPort"]);
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                //TODO: trigger is magic string 
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMqConnectionShutdown;

                _logger.LogInformation($"--> Connected to message bus");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"--> Could not connect to the message bus: {ex.Message}");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body
                );
            _logger.LogInformation($"--> Message sent: {message}");
        }
        public void Dispose()
        {
            _logger.LogInformation("Message bus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMqConnectionShutdown(
            object sender, ShutdownEventArgs events)
        {
            _logger.LogInformation($"--> RabbitMQ connection shutdown");
        }
    }
}
