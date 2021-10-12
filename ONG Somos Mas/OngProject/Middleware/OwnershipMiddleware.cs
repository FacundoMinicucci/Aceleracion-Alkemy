using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Middleware
{
    public class OwnershipMiddleware
    {
        private readonly RequestDelegate _next;
        public OwnershipMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            string path = context.Request.Path;
            if (path.Contains("Users"))
            {
                string[] splittedPath = path.Split("/");
                var id = context.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                var rol = context.User.IsInRole("Administrator");
                if (splittedPath.Length < 3)
                {
                    await _next.Invoke(context);
                }
                else if (splittedPath[2] == id || rol)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 403;
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
