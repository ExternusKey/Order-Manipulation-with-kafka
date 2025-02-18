using Microsoft.AspNetCore.Mvc;
using ServicesManipulation.Messaging;
using ServicesManipulation.Models;

namespace ServicesManipulation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ClientOrderProducer _clientOrderProducer = new();

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] OrderRequest request)
    {
        await _clientOrderProducer.SendOrderAsync(request);
        return Ok();
    }
}