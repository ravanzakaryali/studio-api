using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Space.WebAPI.Filters;

//Todo: Authentication Requriment Summary
/// <summary>
/// 
/// </summary>
public class AuthenticationRequirementOperationFilter : IOperationFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.GetCustomAttributes(true)
            .Union(context.MethodInfo.DeclaringType?.GetCustomAttributes(true) ?? Enumerable.Empty<object>())
            .OfType<AuthorizeAttribute>()
            .Any();
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


//Todo: EnumAsSchemaFilter  summary
/// <summary>
/// 
/// </summary>
public class EnumAsSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var typeInfo = context.Type.GetTypeInfo();

        // Check if the type is an enum
        if (typeInfo.IsEnum)
        {
            schema.Enum.Clear();
            schema.Type = "string";
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