using CommandService.Models;

namespace CommandService.Services
{
    public interface IPlatformService
    {
        IEnumerable<Platform> GetAllPlatfroms();
        bool PlatformExists(int platformId);
    }
}
