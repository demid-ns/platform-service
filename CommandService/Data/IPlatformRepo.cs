using CommandService.Models;

namespace CommandService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool PlatfromExists(int id);
        bool ExternalPlatformExists(int externalPlatformId);
    }
}
