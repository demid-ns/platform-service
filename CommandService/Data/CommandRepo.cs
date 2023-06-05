using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _db;

        public CommandRepo(AppDbContext db)
        {
            _db = db;
        }

        public void CreateCommand(Command command)
        {
            if (command != null)
                throw new ArgumentNullException(nameof(command));

            _db.Commands.Add(command);
        }

        public Command GetCommand(int platfromId, int commandId)
        {
            var command = _db.Commands
                .Where(c => c.PlatformId == platfromId && c.Id == commandId)
                .SingleOrDefault();
            return command;
        }

        public IEnumerable<Command> GetAllCommandsForPlatform(int platformId)
        {
            var commands = _db.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
            return commands;
        }

        public bool SaveChanges()
        {
            var anyChanges = _db.SaveChanges() >= 0;
            return anyChanges;
        }
    }
}
