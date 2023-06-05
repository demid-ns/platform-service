using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Services
{
    public interface IPlatformService
    {
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        Task<int> CreatePlatformAsync(Platform platform);
    }
}
