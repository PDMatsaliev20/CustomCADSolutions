using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                if (!context.ApiDescription.CustomAttributes().Any((a) => a is AllowAnonymousAttribute)
                    && (context.ApiDescription.CustomAttributes().Any((a) => a is AuthorizeAttribute)
                        || descriptor.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>() != null))
                {
                    operation.Security ??= [];

                    operation.Security.Add(new()
                    {
                        {
                            new()
                            {
                                Name = "Authorization", 
                                In = ParameterLocation.Header, 
                                BearerFormat = "Bearer token", 
                                Reference = new OpenApiReference 
                                {
                                    Type = ReferenceType.SecurityScheme, 
                                    Id = "Bearer" 
                                }
                            },
                            [] 
                        } 
                    });
                }
            }
        }
    }
}