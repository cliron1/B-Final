using BezeqFinalProject.WebApi.Config;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BezeqFinalProject.WebApi.Config {
    public static class SwaggerConfig {
        public static IServiceCollection AddSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("main", new OpenApiInfo { Title = "Main API", Version = "v1", Contact = new OpenApiContact { Email = "liron@flame-ware.com" } });

                c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BezeqFinalProject.WebApi.xml"));

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                var security = new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { security, new List<string>() } });
            });
            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app) {
            app.UseSwagger(c => { c.RouteTemplate = "{documentName}.json"; });
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/main.json", "Main API");
                c.RoutePrefix = "docs";
                c.DocExpansion(DocExpansion.None);
                //c.InjectJavascript("/js/swagger.js?v=1");
                //c.SupportedSubmitMethods(SubmitMethod.Get);
            });
            return app;
        }
    }
}