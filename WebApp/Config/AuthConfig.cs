using Microsoft.AspNetCore.Authentication.Cookies;

namespace BezeqFinalProject.WebApp.Config;

public static class AuthConfig {
    public static IServiceCollection AddAuth(this IServiceCollection services) {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
            });
        return services;
    }
}
