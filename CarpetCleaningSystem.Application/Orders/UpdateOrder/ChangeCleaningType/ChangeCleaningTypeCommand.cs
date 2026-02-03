using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CarpetCleaningSystem.Domain.Enums;

public class ChangeCleaningTypeCommand
{
    [JsonIgnore]
    public int OrderId { get; set; }
    
    [JsonIgnore]
    public int ItemNo { get; set; }

    [Required]
    [EnumDataType(typeof(CleaningType), ErrorMessage = "Cleaning type must be a valid value.")]
    public CleaningType CleaningType { get; set; }
}

