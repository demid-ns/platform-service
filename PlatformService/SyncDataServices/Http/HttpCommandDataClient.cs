using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpCommandDataClient> _logger;

        public HttpCommandDataClient(
            HttpClient httpClient,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<HttpCommandDataClient> logger)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPlatformToCommand(Platform plat)
        {
            var platformDto = _mapper.Map<PlatformReadDto>(plat);

            var httpContent = new StringContent(
                JsonSerializer.Serialize(platformDto),
                Encoding.UTF8,
                "application/json");

            string url = $"{_configuration["CommandService"]}/api/c/platforms";
            var response = await _httpClient.PostAsync(url, httpContent);

            if (response.IsSuccessStatusCode)
                _logger.LogInformation("--> Sync POST to CommandService was OK");
            else
                _logger.LogInformation("--> Sync POST to CommandService was NOT OK");
        }
    }
}
