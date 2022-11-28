using AirlyMonitor.Models.Configuration;
using Microsoft.OpenApi.Models;

namespace AirlyMonitor.Extensions
{
    public static class SwaggerExtentions
    {
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                var scheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(configuration.GetSection("Swagger:AuthorizationUrl").Get<string>()),
                            TokenUrl = new Uri(configuration.GetSection("Swagger:TokenUrl").Get<string>())
                        }
                    },
                    Type = SecuritySchemeType.OAuth2
                };

                options.AddSecurityDefinition("OAuth", scheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
                        },
                        new List<string> { }
                    }
                });
            });
        }

        public static void AddSwaggerUI(this WebApplication app, IConfiguration configuration) 
        {
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
                options.OAuthClientId(configuration.GetSection("Swagger:Client").Get<string>());
                options.OAuthClientSecret(configuration.GetSection("Swagger:Secret").Get<string>());
                options.OAuthScopes(configuration.GetSection("Swagger:Scopes").Get<string>());
                options.OAuthUsePkce();
                options.EnablePersistAuthorization();
            });
        }
    }
}
