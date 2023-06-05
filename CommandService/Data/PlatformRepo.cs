using CommandService.Models;

namespace CommandService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _db;

        public PlatformRepo(AppDbContext db)
        {
            _db = db;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null)
                throw new ArgumentException(nameof(platform));

            _db.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _db.Platforms.ToList();
        }

        public bool PlatfromExists(int id)
        {
            var anyPlatform = _db.Platforms.Any(p => p.Id == id);
            return anyPlatform;
        }

        public bool ExternalPlatformExists(int externalPlatformId)
        {
            return _db.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }

        public bool SaveChanges()
        {
            var anyChanges = _db.SaveChanges() >= 0;
            return anyChanges;
        }
    }
}
