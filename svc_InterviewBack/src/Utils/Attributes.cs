using System.ComponentModel.DataAnnotations;

namespace svc_InterviewBack.Utils;

public class SeasonYearRangeAttribute : RangeAttribute
{
    public SeasonYearRangeAttribute() : base(2010, 3000)
    {
        ErrorMessage = "SeasonYear must be between 2010 and 3000";
    }
}