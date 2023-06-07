using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubsriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<MessageBusSubsriber> _logger;

        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubsriber(
            IConfiguration configuration,
            IEventProcessor eventProcessor,
            ILogger<MessageBusSubsriber> logger
            )
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            _logger = logger;

            SetupConnection();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ModuleHandle, ea) =>
            {
                _logger.LogInformation("--> Event received");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
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
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");
                _connection.ConnectionShutdown += RabbitMqConnectionShutdown;

                _logger.LogInformation($"--> Listening to message bus");
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Could not connect to the message bus: {ex.Message}");
            }
        }

        private void RabbitMqConnectionShutdown(object sender, ShutdownEventArgs events)
        {
            _logger.LogInformation($"--> RabbitMQ connection shutdown");
        }

        public override void Dispose()
        {
            _logger.LogInformation("Message bus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
