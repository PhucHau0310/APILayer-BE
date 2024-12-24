using APILayer.Services.Interfaces;
using APILayer.Security;

namespace APILayer.Middlewares
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RoleMiddleware> _logger;

        public RoleMiddleware(RequestDelegate next, ILogger<RoleMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip favicon requests
            if (context.Request.Path == "/favicon.ico")
            {
                await _next(context);
                return;
            }

            // Handle OPTIONS requests (CORS preflight)
            if (context.Request.Method == "OPTIONS")
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path.Value.ToLower();
            _logger.LogInformation($"Processing request for path: {path}");

            Console.WriteLine(Endpoints.PublicEndpoints.Any(e => path.StartsWith(e)));

            // Check if it's a public endpoint
            if (Endpoints.PublicEndpoints.Any(e => path.StartsWith(e)))
            {
                _logger.LogInformation($"Allowing access to public endpoint: {path}");
                await _next(context);
                return;
            }

            // Skip auth check for authentication endpoints
            if (!context.User.Identity.IsAuthenticated)
            {
                _logger.LogWarning($"Unauthorized access attempt to: {path}");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            using (var scope = context.RequestServices.CreateScope())
            {
                var expClaim = context.User.FindFirst("exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out var expSeconds))
                {
                    var tokenExpiryDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                    if (tokenExpiryDate < DateTime.UtcNow)
                    {
                        var authService = context.RequestServices.GetRequiredService<IAuthService>();
                        var refreshToken = context.Request.Headers["X-Refresh-Token"].ToString();

                        if (string.IsNullOrEmpty(refreshToken))
                        {
                            _logger.LogWarning("Refresh token is missing in request headers.");
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Unauthorized - Refresh token required");
                            return;
                        }

                        var newAccessToken = authService.RefreshTokenAsync(refreshToken);

                        if (newAccessToken == null)
                        {
                            _logger.LogWarning("Failed to refresh access token - Invalid or expired refresh token.");
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Unauthorized - Invalid refresh token");
                            return;
                        }

                        context.Response.Headers["X-New-Access-Token"] = newAccessToken.Result.AccessToken;
                        _logger.LogInformation("Access token refreshed successfully.");
                    }
                }

                bool isAdminEndpoint = Endpoints.AdminEndpoints.Any(e => path.StartsWith(e));
                bool isFreelancerEndpoint = Endpoints.CustomerEndpoints.Any(e => path.StartsWith(e));
                bool isClientEndpoint = Endpoints.ProviderEndpoints.Any(e => path.StartsWith(e));

                bool hasAccess = false;

                if (isAdminEndpoint && context.User.IsInRole("Admin"))
                    hasAccess = true;

                if (isFreelancerEndpoint && context.User.IsInRole("Customer"))
                    hasAccess = true;

                if (isClientEndpoint && context.User.IsInRole("Provider"))
                    hasAccess = true;

                if (!hasAccess)
                {
                    _logger.LogWarning($"Forbidden access attempt to role-specific endpoint: {path}");
                    context.Response.StatusCode = 403; // Forbidden
                    await context.Response.WriteAsync("Forbidden - Insufficient role permissions");
                    return;
                }
            }

            await _next(context);
        }
    }
}
