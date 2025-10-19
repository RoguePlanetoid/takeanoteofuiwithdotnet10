namespace AspNetCoreWebApiNotes.Attributes;

/// <summary>
/// Allowed Background Attribute
/// </summary>
public class AllowedBackgroundAttribute : ValidationAttribute
{
    /// <summary>
    /// Is Valid
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="context">Validation Contect</param>
    /// <returns>Validation Result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(value as string))
            return new ValidationResult("Value is required.");
        var compare = ((string)value).Trim();
        var (_, background) = BackgroundModel.Options.FirstOrDefault(c =>
            string.Equals(c.Name, compare, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.Background, compare, StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrEmpty(background))
            return new ValidationResult($"Value must be one of: {BackgroundModel.NamesOutput}");
        return ValidationResult.Success;
    }
}