using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicesManipulation.Models;

public class GpuModel()
{
    public int Id { get; set; }
    public string Name { get; set; }
}