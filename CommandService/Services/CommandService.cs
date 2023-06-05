using CommandService.Data;
using CommandService.Models;

namespace CommandService.Services
{
    public class CommandService : ICommandService
    {
        private readonly ICommandRepo _repo;

        public CommandService(
            ICommandRepo repo
            )
        {
            _repo = repo;
        }

        public int CreateCommand(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _repo.CreateCommand(command);
            _repo.SaveChanges();

            return command.Id;
        }

        public IEnumerable<Command> GetAllCommandsForPlatform(int platformId)
        {
            return _repo.GetAllCommandsForPlatform(platformId);
        }

        public Command GetCommandForPlatform(int platformId, int commandId)
        {
            return _repo.GetCommand(platformId, commandId);
        }
    }
}
