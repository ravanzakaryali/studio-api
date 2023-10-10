using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Space.WebAPI.Filters;

/// <summary>
/// Operation filter for Swagger/OpenAPI to apply security requirements for endpoints with the [Authorize] attribute.
/// </summary>
public class AuthenticationRequirementOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies security requirements to Swagger/OpenAPI operations based on the presence of [Authorize] attribute.
    /// </summary>
    /// <param name="operation">The Swagger/OpenAPI operation to which security requirements will be applied.</param>
    /// <param name="context">The context containing information about the API operation.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if the method or its declaring type has the [Authorize] attribute
        var hasAuthorize = context.MethodInfo.GetCustomAttributes(true)
            .Union(context.MethodInfo.DeclaringType?.GetCustomAttributes(true) ?? Enumerable.Empty<object>())
            .OfType<AuthorizeAttribute>()
            .Any();

        // If [Authorize] attribute is present, add Bearer token security requirement
        if (hasAuthorize)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var authRequirements = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            };

            operation.Security.Add(authRequirements);
        }
    }
}



/// <summary>
/// Schema filter for Swagger/OpenAPI to represent enums as strings with display names.
/// </summary>
public class EnumAsSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Applies schema filtering to represent enums as strings with display names.
    /// </summary>
    /// <param name="schema">The OpenAPI schema to which filtering is applied.</param>
    /// <param name="context">The context containing information about the schema type.</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var typeInfo = context.Type.GetTypeInfo();

        // Check if the type is an enum
        if (typeInfo.IsEnum)
        {
            schema.Enum.Clear(); // Clear existing enum values
            schema.Type = "string"; // Represent enum as a string

            // Iterate through enum values and add them as strings with display names
            foreach (var enumValue in Enum.GetValues(context.Type))
            {
                var enumMemberInfo = context.Type.GetMember(enumValue.ToString()!).First();
                var enumMemberAttribute = enumMemberInfo.GetCustomAttribute<EnumMemberAttribute>();
                var enumDisplayName = enumMemberAttribute?.Value ?? enumValue.ToString();
                schema.Enum.Add(new OpenApiString(enumDisplayName));
            }
        }
    }
}
