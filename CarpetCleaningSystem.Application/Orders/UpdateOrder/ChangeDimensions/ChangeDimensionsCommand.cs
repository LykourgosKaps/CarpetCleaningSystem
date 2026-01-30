using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ChangeDimensionsCommand
{
    [JsonIgnore]
    [Required, Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [JsonIgnore]
    [Required, Range(1, int.MaxValue)]
    public int ItemNo { get; set; }

    [Required, Range(0.01, 100.00)]
    public decimal Width { get; set; }

    [Required, Range(0.01, 100.00)]
    public decimal Length { get; set; }
}

