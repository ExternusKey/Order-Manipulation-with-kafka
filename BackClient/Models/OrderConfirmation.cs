using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbClientService.Models;

[Table("order_confirmation")]
public class OrderConfirmation
{
    [Key]
    [Display(Name = "Order ID")]
    [Column("order_id")]
    public string OrderId { get; set; }
    
    [Required]
    [Display(Name = "Product ID")]
    [Column("product_id")]
    public string ProductId { get; set; }
    
    [Required]
    [Display(Name = "UserName")]
    [Column("user_name")]
    public string UserName {get; set; }
    
    [Display(Name = "Processed by")]
    [Column("processed_by")]
    public string ProcessedBy {get; set; }
    
    [Display(Name = "Confirmed At")]
    [Column("confirmed_at")]
    public string ConfirmedAt { get; set; }
}