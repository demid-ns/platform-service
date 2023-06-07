using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using System.Text.Json;

namespace CommandService.EventProcessing
{
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }

    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;

        public EventProcessor(
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            ILogger<EventProcessor> logger
            )
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default: 
                    break;
            }
        }

        private EventType DetermineEvent(string message)
        {
            _logger.LogInformation("--> Determining event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);
            switch (eventType.Event)
            {
                case "Platform_Published":
                    _logger.LogInformation("--> Platform published event detected");
                    return EventType.PlatformPublished;
                default:
                    _logger.LogInformation("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using var scope = _scopeFactory.CreateScope();

            var repo = scope.ServiceProvider.GetRequiredService<IPlatformRepo>();

            var platformPublishedDto = JsonSerializer
                .Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);
                bool platformExist = repo.PlatfromExists(platform.ExternalId);
                if (!platformExist)
                {
                    repo.CreatePlatform(platform);
                    repo.SaveChanges();
                    _logger.LogInformation($"--> Platform added");
                }
                else
                {
                    _logger.LogInformation($"--> Platform already exist");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Could not add platform to DB: {ex.Message}");
            }
        }
    }
}
