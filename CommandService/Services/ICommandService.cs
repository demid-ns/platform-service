using CommandService.Models;

namespace CommandService.Services
{
    public interface ICommandService
    {
        int CreateCommand(Command command);
        IEnumerable<Command> GetAllCommandsForPlatform(int platformId);
        Command GetCommandForPlatform(int platformId, int commandId);
    }
}
