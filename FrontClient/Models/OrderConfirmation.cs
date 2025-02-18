using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManipulation.Models;

public class OrderConfirmation
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public string GpuName { get; set; }
    public string UserName { get; set; }
    public string ProcessedBy { get; set; }
    public string ConfirmedAt { get; set; }
}
