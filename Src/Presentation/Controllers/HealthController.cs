using Microsoft.AspNetCore.Mvc;

namespace PrismodInventory.Src.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            service = "prismodInventory",
            status = "ok",
            timestampUtc = DateTime.UtcNow
        });
    }
}
