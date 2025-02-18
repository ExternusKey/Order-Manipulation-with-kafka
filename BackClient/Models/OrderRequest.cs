using System.ComponentModel.DataAnnotations;

namespace DbClientService.Models;

public class OrderRequest
{
    [Key]
    public string OrderId { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string ProductId { get; set; }
    [Required]
    public string UserName { get; set; }
    public string OrderDate { get; set; }
}