using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CarpetCleaningSystem.Domain.Enums;

public class ChangeCleaningTypeCommand
{
    [JsonIgnore]
    [Required, Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [JsonIgnore]
    [Required, Range(1, int.MaxValue)]
    public int ItemNo { get; set; }

    [Required]
    public CleaningType CleaningType { get; set; }
}

