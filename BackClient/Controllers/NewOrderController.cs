using DbClientService.Data;
using DbClientService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DbClientService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewOrderController(IServiceScopeFactory scopeFactory) : ControllerBase
{
    
    [HttpPost]
    public async Task<IActionResult> PostNewOrderData([FromBody] OrderRequest orderRequest)
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.OrderRequests.Add(orderRequest);
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error when try to post New Order", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostConfirmation([FromBody] OrderConfirmation orderConfirmation)
    {
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
            return StatusCode(500, new { message = "Error when try to post confirmation", details = ex.Message });
        }
    }
}