using System.ComponentModel.DataAnnotations;

namespace ServicesManipulation.Models;

public class OrderRequest
{
    [Key]
    public string? OrderId { get;} = Guid.NewGuid().ToString();
    [Required]
    public string ProductId { get; set; }
    [Required]
    public string UserName { get; set; }
    
    public string OrderDate { get; set; }
    
}