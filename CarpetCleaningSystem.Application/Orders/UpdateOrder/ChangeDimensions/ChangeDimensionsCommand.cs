using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ChangeDimensionsCommand
{
    
    
    [JsonIgnore]
    public int OrderId { get; set; }

    
    
    [JsonIgnore]
    public int ItemNo { get; set; }

    [Required, Range(0.01, 100.00)]
    public decimal Width { get; set; }

    [Required, Range(0.01, 100.00)]
    public decimal Length { get; set; }
}

