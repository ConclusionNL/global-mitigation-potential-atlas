using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GMPA.Core.Swagger
{
    public class EnumNamesSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (!type.IsEnum || schema.Extensions.ContainsKey("x-enumNames"))
            {
                return;
            }

            var values = new OpenApiArray();
            values.AddRange(Enum.GetNames(context.Type).Select(value => new OpenApiString(value)));

            schema.Extensions.Add(
                "x-enumNames",
                values
            );
        }
    }
}