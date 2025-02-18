using System.ComponentModel.DataAnnotations;

namespace ServicesManipulation.Models;

public class OrderRequest
{
    [Key]
    public string OrderId { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string ProductId { get; set; }
    [Required]
    public string UserName { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;
    
}