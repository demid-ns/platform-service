using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.Services;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly IMapper _mapper;

        public PlatformsController(
            IPlatformService platformService,
            IMapper mapper)
        {
            _platformService = platformService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms()
        {
            var platforms = _platformService.GetAllPlatforms();
            var result = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platform = _platformService.GetPlatformById(id);
            if (platform == null) return NotFound();

            var result = _mapper.Map<PlatformReadDto>(platform);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform
            ([FromBody] PlatformCreateDto dto)
        {
            var platform = _mapper.Map<Platform>(dto);
            await _platformService.CreatePlatformAsync(platform);

            var readDto = _mapper.Map<PlatformReadDto>(platform);
            return CreatedAtAction(nameof(GetPlatformById), new { id = readDto.Id }, readDto);
        }
    }
}
