using System.ComponentModel.DataAnnotations;
using CarpetCleaningSystem.Domain.Enums;

public class ChangeCleaningTypeCommand
{
    [Required, Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int OrderItemId { get; set; }

    [Required]
    public CleaningType CleaningType { get; set; }
}

