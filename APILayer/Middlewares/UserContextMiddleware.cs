namespace APILayer.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;
            if (user.Identity.IsAuthenticated)
            {
                context.Items["User"] = user;
            }

            await _next(context);
        }
    }
}
