using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;
using CommandService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandService _commandsService;
        private readonly IPlatformService _platformService;
        private readonly IMapper _mapper;

        public CommandsController(
            ICommandService commandsService,
            IPlatformService platformService,
            IMapper mapper
            )
        {
            _commandsService = commandsService;
            _platformService = platformService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommandsForPlatform(int platformId)
        {
            bool platformExist = _platformService.PlatformExists(platformId);
            if (!platformExist) return NotFound();

            var commands = _commandsService.GetAllCommandsForPlatform(platformId);
            var commandDtos = _mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return Ok(commandDtos);
        }

        [HttpGet("{commandId}")]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandForPlatform(
            int platformId,
            int commandId
            )
        {
            var command = _commandsService.GetCommandForPlatform(platformId, commandId);
            if (command == null) return NotFound();

            var commandDto = _mapper.Map<CommandReadDto>(command);
            return Ok(commandDto);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(
            int platformId,
            [FromBody] CommandCreateDto dto
            )
        {
            bool platformExist = _platformService.PlatformExists(platformId);
            if (!platformExist) return NotFound();

            var command = _mapper.Map<Command>(dto);
            command.PlatformId = platformId;
            _commandsService.CreateCommand(command);

            var readDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtAction(nameof(GetCommandForPlatform), 
                new { platformId = platformId, commandId = command.Id }, 
                readDto);
        }
    }
}
