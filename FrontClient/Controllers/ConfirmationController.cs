using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ServicesManipulation.Models;

namespace ServicesManipulation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfirmationController : Controller
{
    [HttpPost]
    public async Task<IActionResult> SendOrderConfirmationToDbAsync(OrderConfirmation orderConfirmation)
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.PostAsync(
                "http://localhost:59291/api/Confirmation", 
                new StringContent(JsonSerializer.Serialize(orderConfirmation), Encoding.UTF8, "application/json")
            );
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error in POST confirmation request", details = ex.Message });
        }
    }
}