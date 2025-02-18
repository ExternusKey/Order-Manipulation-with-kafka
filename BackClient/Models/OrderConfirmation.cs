using System.ComponentModel.DataAnnotations;

namespace DbClientService.Models;

public class OrderConfirmation
{
    [Key]
    public string OrderId { get; set; }
    [Required]
    public string ProductId { get; set; }
    [Required]
    public string UserName {get; set; }

    public string ProcessedBy {get; set; }
    public string ConfirmedAt { get; set; }
    
}

