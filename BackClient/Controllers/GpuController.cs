using DbClientService.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbClientService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GpuController(IServiceScopeFactory scopeFactory) : Controller
{
    [HttpGet] public async Task<IActionResult> GetGpuModelsAsync()
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var gpuModels = await dbContext.GpuModels.ToListAsync();

            if (gpuModels.Count == 0)
                return NotFound(new { message = "No GPU models found." });


            return Ok(gpuModels);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", details = ex.Message });
        }
    }
}