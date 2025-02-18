﻿using System.ComponentModel.DataAnnotations;

namespace ServicesManipulation.Models;

public class Confirmation
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