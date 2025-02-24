using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ServicesManipulation.Models;

namespace ServicesManipulation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfirmationController(IHttpClientFactory httpClientFactory) : Controller
{    
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");

    [HttpPost]
    public async Task<IActionResult> SendOrderConfirmationToDbAsync(OrderConfirmation orderConfirmation)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(orderConfirmation), Encoding.UTF8, "application/json");
        
            Console.WriteLine("Sending request to API...");
            Console.WriteLine($"Request: {JsonSerializer.Serialize(orderConfirmation)}");
        
            var response = await _httpClient.PostAsync("api/Confirmation", content);
        
            Console.WriteLine($"Response Status: {response.StatusCode}");
        
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {responseContent}");
            }
        
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error in POST confirmation request", details = ex.Message });
        }
    }
}