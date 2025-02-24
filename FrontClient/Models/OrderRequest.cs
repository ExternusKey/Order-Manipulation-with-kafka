using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManipulation.Models;

[Table("order_request")]
public class OrderRequest()
{
    public string OrderId { get; set; } = Guid.NewGuid().ToString();
    public string ProductId { get; set; }
    public string UserName { get; set; }
    public string OrderDate { get; set; }
}