using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Interns.Common.SwaggerEnum;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Enum is not { Count: > 0 }) return;
        
        // Extract enum values as strings
        var enumValues = schema.Enum
            .Select(v => v is OpenApiInteger integer ? Enum.GetName(context.Type, integer.Value) : v.ToString())
            .ToList();

        schema.Enum.Clear();
        foreach (var value in enumValues)
        {
            schema.Enum.Add(new OpenApiString(value));
        }
    }
}