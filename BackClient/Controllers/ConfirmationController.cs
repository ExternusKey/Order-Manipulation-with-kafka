using DbClientService.Data;
using DbClientService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DbClientService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfirmationController(IServiceScopeFactory scopeFactory) : Controller
{
    [HttpPost]
    public async Task<IActionResult> PostConfirmation([FromBody] OrderConfirmation orderConfirmation)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.ConfirmedOrders.Add(orderConfirmation);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error when creating confirmation", details = ex.Message });
        }
    }
}