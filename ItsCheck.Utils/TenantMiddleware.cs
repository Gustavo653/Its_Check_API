using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;

namespace ItsCheck.Utils
{
    public class TenantMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = GetClaimValue(context.User, ClaimTypes.NameIdentifier);
                var tenantId = GetClaimValue(context.User, ClaimTypes.PrimaryGroupSid);
                var email = GetClaimValue(context.User, ClaimTypes.Email);
                context.Session.Set(Consts.ClaimUserId, Encoding.UTF8.GetBytes(userId));
                context.Session.Set(Consts.ClaimTenantId, Encoding.UTF8.GetBytes(tenantId));
                context.Session.Set(Consts.ClaimEmail, Encoding.UTF8.GetBytes(email));
            }

            await next(context);
        }

        private static string GetClaimValue(ClaimsPrincipal user, string claimType)
        {
            var claim = user.Claims.FirstOrDefault(x => x.Type == claimType);
            return claim?.Value ?? string.Empty;
        }
    }
}
