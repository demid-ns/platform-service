using AutoMapper;
using CommandService.Dtos;
using CommandService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;

        public PlatformsController(
            IPlatformService platformService,
            IMapper mapper,
            ILogger<PlatformsController> logger
            )
        {
            _platformService= platformService;
            _mapper= mapper;
            _logger= logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatforms() { 
            var platforms = _platformService.GetAllPlatfroms();
            var platformsDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return Ok(platformsDtos);
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            _logger.LogInformation("--> Inbound Post # Command Service");

            return Ok("Inbound test of from Platforms Controller");
        }
    }
}
