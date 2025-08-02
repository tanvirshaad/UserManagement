using UserManagement.Interfaces;

namespace UserManagement.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService)
        {
            // Skip authentication for login and registration pages
            if (context.Request.Path.StartsWithSegments("/Account/Login") || 
                context.Request.Path.StartsWithSegments("/Account/Register"))
            {
                await _next(context);
                return;
            }

            // Check if user is authenticated
            var userId = context.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                context.Response.Redirect("/Account/Login?error=authentication_required");
                return;
            }

            // Check if user is still valid (not blocked or deleted)
            if (!await authService.IsUserValidAsync(userId.Value))
            {
                context.Session.Clear();
                context.Response.Redirect("/Account/Login?error=account_blocked");
                return;
            }

            await _next(context);
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
} 