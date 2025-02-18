namespace DbClientService.Models;

public class OrderRequest
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public string UserName { get; set; }
}