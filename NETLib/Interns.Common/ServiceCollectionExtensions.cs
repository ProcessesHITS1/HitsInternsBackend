using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Interns.Common;

public static class ServiceCollectionExtensions
{
    public static void AddCustomJsonOptions(this  IServiceCollection services )
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        //also add c.SchemaFilter<EnumSchemaFilter>(); in AddSwaggerGen
    }
}