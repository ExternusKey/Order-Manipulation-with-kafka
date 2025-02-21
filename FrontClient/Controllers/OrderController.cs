using Microsoft.AspNetCore.Mvc;
using ServicesManipulation.Messaging;
using ServicesManipulation.Models;

namespace ServicesManipulation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : Controller
{
    private readonly ClientOrderProducer _clientOrderProducer = new();
    
    public IActionResult Index()
    {
        return View("~/Views/Home/Index.cshtml");
    }
    
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] OrderRequest request)
    {
        try
        {
            request.OrderDate = DateTime.Now.ToString("MM/dd/yyyy");
            var orderId = await _clientOrderProducer.SendOrderAsync(request);
            if (orderId != null)
                return Ok(orderId);
            return BadRequest("Failed to send order.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}