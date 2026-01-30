using System.ComponentModel.DataAnnotations;

public class ValidEnumAttribute : ValidationAttribute
{
    private readonly Type _enumType;

    public ValidEnumAttribute(Type enumType)
    {
        _enumType = enumType;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value == null || !Enum.IsDefined(_enumType, value))
        {
            var allowed = string.Join(", ", Enum.GetNames(_enumType));
            return new ValidationResult(
                $"{context.MemberName} must be one of: {allowed}");
        }

        return ValidationResult.Success;
    }
}

