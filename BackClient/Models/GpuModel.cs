using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbClientService.Models;

[Table("gpu_models")]
public class GpuModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("gpu_name")]
    public string Name { get; set; }
}