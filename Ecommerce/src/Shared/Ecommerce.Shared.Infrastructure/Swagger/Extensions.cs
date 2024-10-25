using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Swagger
{
    internal static class Extensions
    {
        public static IServiceCollection AddDocumentation(this IServiceCollection services/*, IApiVersionDescriptionProvider apiVersionDescriptionProvider*/)
        {
            var sp = services.BuildServiceProvider();
            var apiVersionDescriptionProvider = sp.GetRequiredService<IApiVersionDescriptionProvider>();
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                //options.SwaggerDoc("v1", new OpenApiInfo()
                //{
                //    Title = "EcommerceAPI",
                //    Version = "v1",
                //});
                //options.SwaggerDoc("v2", new OpenApiInfo()
                //{
                //    Title = "EcommerceAPI",
                //    Version = "v2",
                //});
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    Console.WriteLine("1");
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo()
                    {
                        Title = "EcommerceAPI",
                        Version = description.ApiVersion.ToString(),
                        Description = description.IsDeprecated ? "This API version has been deprecated." : ""
                    });
                }
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            return services;
        }
        public static WebApplication UseDocumentation(this WebApplication app)
        {
            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"V{description.ApiVersion}");
                }
                //options.SwaggerEndpoint($"/swagger/v1/swagger.json", "V1");
                //options.SwaggerEndpoint($"/swagger/v2/swagger.json", "V2");
            });
            //app.UseSwaggerUI();
            return app;
        }
    }
}
