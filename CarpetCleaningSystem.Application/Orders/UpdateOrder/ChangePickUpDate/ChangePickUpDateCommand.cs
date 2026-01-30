using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ChangePickUpDateCommand
{
    [JsonIgnore]
    [Required, Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Required]
    public DateTime PickUpDate { get; set; }
}
