using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class DbInit
    {
        private static readonly ILogger _logger;

        static DbInit()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            _logger = loggerFactory.CreateLogger(typeof(DbInit));
        }

        public static void Initialize(WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();
            var dbContext = serviceScope.ServiceProvider
                .GetRequiredService<AppDbContext>();
            SeedData(dbContext);
        }

        private static void SeedData(AppDbContext context)
        {
            try
            {
                _logger.LogInformation("--> Applying migrations");
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Migrations failed: {ex.Message}");
            }

            var anyData = context.Platforms.Any();
            if (anyData) return;

            var platforms = new List<Platform> {
                new Platform { Name = "Dot Net", Pubslisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "SQL Server Express", Pubslisher = "Microsoft", Cost = "Free" },
                new Platform { Name = "Kubernetes", Pubslisher = "Cloud Native Computing Foundation", Cost = "Free"},
                new Platform { Name = "Docker", Pubslisher = "Docker", Cost = "Free"}
            };
            context.Platforms.AddRange(platforms);
            context.SaveChanges();
        }
    }
}
