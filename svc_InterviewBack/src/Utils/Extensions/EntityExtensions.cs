namespace svc_InterviewBack.Utils.Extensions;

public static class EntityExtensions
{
    public static void UpdateProperties<T>(this T entity, T updateData) where T : class
    {
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var newValue = property.GetValue(updateData);
            if (newValue != null && !(newValue is Guid guidValue && guidValue == Guid.Empty))
            {
                property.SetValue(entity, newValue);
            }
        }
    }
}