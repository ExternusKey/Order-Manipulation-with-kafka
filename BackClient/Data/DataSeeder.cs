using DbClientService.Models;

namespace DbClientService.Data;

public static class DataSeeder
{
    public static void SeedGpuData(AppDbContext context)
    {
        if (context.GpuModels.Any()) return;
        
        context.GpuModels.AddRange(
            new GpuModel { Name = "NVIDIA GeForce RTX 3080" },
            new GpuModel { Name = "AMD Radeon RX 6800 XT" },
            new GpuModel { Name = "NVIDIA GeForce GTX 1660" },
            new GpuModel { Name = "NVIDIA GeForce RTX 4090" },
            new GpuModel { Name = "AMD Radeon RX 7900 XTX" },
            new GpuModel { Name = "NVIDIA GeForce RTX 3070" },
            new GpuModel { Name = "AMD Radeon RX 5700 XT" },
            new GpuModel { Name = "NVIDIA GeForce GTX 1050 Ti" },
            new GpuModel { Name = "AMD Radeon RX Vega 64" },
            new GpuModel { Name = "NVIDIA Quadro RTX 8000" },
            new GpuModel { Name = "NVIDIA Titan RTX" },
            new GpuModel { Name = "AMD Radeon RX 5600 XT" },
            new GpuModel { Name = "NVIDIA GeForce RTX 2060 Super" }
        );
        context.SaveChanges();
    }
}