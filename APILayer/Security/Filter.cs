using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace APILayer.Security
{
    public class Filter : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public Filter(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var path = context.HttpContext.Request.Path.Value.ToLower();

            if (_roles.Contains("Admin") && Endpoints.AdminEndpoints.Any(e => path.StartsWith(e.ToLower())))
            {
                if (!user.IsInRole("Admin"))
                {
                    context.Result = new ForbidResult();
                }
            }
            else if (_roles.Contains("Customer") && Endpoints.CustomerEndpoints.Any(e => path.StartsWith(e.ToLower())))
            {
                if (!user.IsInRole("Customer"))
                {
                    context.Result = new ForbidResult();
                }
            }
            else if (_roles.Contains("Provider") && Endpoints.ProviderEndpoints.Any(e => path.StartsWith(e.ToLower())))
            {
                if (!user.IsInRole("Provider"))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
