using System;
using System.ComponentModel.DataAnnotations;

public class ChangePickUpDateCommand
{
    [Required, Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Required]
    public DateTime PickUpDate { get; set; }
}
