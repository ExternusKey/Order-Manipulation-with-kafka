using System.Text.Json;
using ServicesManipulation.Models;

namespace ServicesManipulation.Data;

public class GpuModelService(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ApiClient");
    public async Task<List<GpuModel>?> GetGpuModels()
    {
        
        var response = await _httpClient.GetAsync("api/Gpu");

        if (!response.IsSuccessStatusCode) return [];
        
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<GpuModel>>(json);

    }
}