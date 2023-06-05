using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();
        IEnumerable<Command> GetAllCommandsForPlatform(int platformId);
        Command GetCommand(int platfromId, int commandId);
        void CreateCommand(Command command);
    }
}
