using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsContoller : ControllerBase
{
    private IPlatformRepo _platformRepo;

    public PlatformsContoller(IPlatformRepo platformRepo)
    {
        _platformRepo = platformRepo;
    }


    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<Platform> GetPlatformById(string id)
    {
        var platform = _platformRepo.GetPlatformById(id);
        if (platform != null)
        {
            return Ok(platform);
        }

        return NotFound();
    }

    [HttpPost("CreatePlatform")]
    public ActionResult<Platform> CreatePlatform(Platform platform)
    {
        _platformRepo.CreatePlatform(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new
        {
            Id = platform.Id
        }, platform);
    }

    [HttpGet("GetAll")]
    public ActionResult<IEnumerable<Platform>> GetAll()
    {
        var platforms = _platformRepo.GetAllPlatform();
        return Ok(platforms);
    }
}