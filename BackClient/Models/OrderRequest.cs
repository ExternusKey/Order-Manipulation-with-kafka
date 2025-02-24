using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbClientService.Models;

[Table("order_request")]
public class OrderRequest
{
    [Key]
    [Display(Name = "Order ID")]
    [Column("order_id")]
    public string OrderId { get; set; } = Guid.NewGuid().ToString();
    
    [Display(Name = "Product ID")]
    [Column("product_id")]
    [Required]
    public string ProductId { get; set; }
    
    [Display(Name = "User Name")]
    [Column("user_name")]
    [Required]
    public string UserName { get; set; }
    
    [Display(Name = "Order Date")]
    [Column("order_date")]
    public string OrderDate { get; set; }
    
}