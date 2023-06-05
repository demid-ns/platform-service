using CommandService.Data;
using CommandService.Models;

namespace CommandService.Services
{
    public class PlatformService: IPlatformService
    {
        private readonly IPlatformRepo _repo;

        public PlatformService(IPlatformRepo repo)
        {
            _repo = repo;
        }

        public IEnumerable<Platform> GetAllPlatfroms()
        {
            return _repo.GetAllPlatforms();
        }

        public bool PlatformExists(int platformId)
        {
            return _repo.PlatfromExists(platformId);
        }
    }
}
