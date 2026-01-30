using System.ComponentModel.DataAnnotations;

public class ChangeDimensionsCommand
{
    [Required, Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int OrderItemId { get; set; }

    [Required, Range(0.01, 100.00)]
    public decimal Width { get; set; }

    [Required, Range(0.01, 100.00)]
    public decimal Length { get; set; }
}

