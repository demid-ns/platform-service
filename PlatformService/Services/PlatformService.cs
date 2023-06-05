using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Services
{
    public class PlatformService: IPlatformService
    {
        private readonly IPlatformRepo _repo;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;
        private readonly ILogger<PlatformService> _logger;

        public PlatformService(
            IPlatformRepo repo,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient,
            ILogger<PlatformService> logger)
        {
            _repo = repo;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
            _logger = logger;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _repo.GetAllPlatforms();
        }

        public Platform GetPlatformById(int id)
        {
            return _repo.GetPlatformById(id);
        }

        public async Task<int> CreatePlatformAsync(Platform platform)
        {
            if(platform== null)
                throw new ArgumentNullException(nameof(platform));

            _repo.CreatePlatform(platform);
            _repo.SaveChanges();

            //Send by http
            try
            {
                await _commandDataClient.SendPlatformToCommand(platform);
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"--> Could not send via http: {ex.Message}");
            }

            //Send by message bus
            try
            {
                _messageBusClient.PublishNewPlatform(platform);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"--> Could not send asynchronously: {ex.Message}");
            }

            return platform.Id;
        }
    }
}
