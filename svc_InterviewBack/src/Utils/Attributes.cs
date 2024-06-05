using System.ComponentModel.DataAnnotations;

namespace svc_InterviewBack.Utils;

public class SeasonYearRangeAttribute : RangeAttribute
{
    public SeasonYearRangeAttribute() : base(2010, 3000)
    {
        ErrorMessage = "SeasonYear must be between 2010 and 3000";
    }
}

public class YearGreaterThan2000Attribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is int year && year <= 2000)
        {
            return new ValidationResult("Year must be greater than 2000.");
        }
        return ValidationResult.Success;
    }
}
