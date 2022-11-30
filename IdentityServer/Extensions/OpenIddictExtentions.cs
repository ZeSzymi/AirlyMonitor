using IdentityServer.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IdentityServer.Extensions
{
    public static class OpenIddictExtentions
    {
        public static void AddOpenIddictOptions(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/account/login";
                });

            services.AddOpenIddict()
               .AddCore(options =>
               {
                   options.UseEntityFrameworkCore()
                       .UseDbContext<IdentityDbContext>();
               })
               .AddServer(options =>
               {
                   options
                        .SetAuthorizationEndpointUris("/connect/authorize")
                        .SetTokenEndpointUris("/connect/token")
                        .SetUserinfoEndpointUris("/connect/userinfo")
                        .SetIntrospectionEndpointUris("/connect/introspect");

                   options
                        .AllowClientCredentialsFlow()
                        .AllowAuthorizationCodeFlow()
                        .RequireProofKeyForCodeExchange();

                   options
                        .AddDevelopmentSigningCertificate()
                       .AddDevelopmentEncryptionCertificate()
                       .DisableAccessTokenEncryption();

                   options.RegisterScopes("api");

                   options.SetAccessTokenLifetime(TimeSpan.FromDays(360));
                   options.SetIdentityTokenLifetime(TimeSpan.FromDays(360));

                   options
                    .UseAspNetCore()
                    .DisableTransportSecurityRequirement()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough();
               });
        }
    }
}
