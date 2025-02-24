using Microsoft.AspNetCore.Mvc;
using ServicesManipulation.Messaging;
using ServicesManipulation.Models;

namespace ServicesManipulation.Controllers;

[ApiController]
[Route("api/[controller]")]
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
            return BadRequest();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error in POST new order request", details = ex.Message });
        }
    }
}