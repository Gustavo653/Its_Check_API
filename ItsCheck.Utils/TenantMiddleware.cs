using Microsoft.AspNetCore.Http;
using System.Text;

namespace ItsCheck.Utils
{
    public class TenantMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Claims.Any())
            {
                context.Session.Set("userId", Encoding.UTF8.GetBytes(context.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? ""));
                context.Session.Set("tenantId", Encoding.UTF8.GetBytes(context.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid")?.Value ?? ""));
            }
            return next(context);
        }
    }
}
