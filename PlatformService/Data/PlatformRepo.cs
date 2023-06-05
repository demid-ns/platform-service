using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _db;

        public PlatformRepo(AppDbContext context)
        {
            _db = context;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _db.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            var platforms = _db.Platforms.ToList();
            return platforms;
        }

        public Platform GetPlatformById(int id)
        {
            var platform = _db.Platforms.SingleOrDefault(p => p.Id == id);
            return platform;
        }

        public bool SaveChanges()
        {
            var anyChanges = _db.SaveChanges() >= 0;
            return anyChanges;
        }
    }
}
