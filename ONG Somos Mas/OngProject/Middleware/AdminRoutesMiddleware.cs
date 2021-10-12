using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Middleware
{
    public class AdminRoutesMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminRoutesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            List<string> methods = new List<string>();
            methods.Add("put");
            methods.Add("post");
            methods.Add("patch");
            methods.Add("delete");

            var method = context.Request.Method;

            List<string> paths = new List<string>();
            paths.Add("/activities");
            paths.Add("/activites/getall");
            paths.Add("/activites/{id}");
            paths.Add("/categories");
            paths.Add("/categories/{id}");                      
            paths.Add("/comments/{id}");            
            paths.Add("/members/{id}");
            paths.Add("/news");
            paths.Add("/news/getall");
            paths.Add("/news/{id}");
            paths.Add("/news/{id}/comments");           
            paths.Add("/organization/public");
            paths.Add("/rol");
            paths.Add("/rol/getall");
            paths.Add("/rol/{id}");            
            paths.Add("/slides");
            paths.Add("/slides/{id}");
            paths.Add("/testimonials");
            paths.Add("/testimonials/getall");
            paths.Add("/testimonials/{id}");
            paths.Add("/users");
            paths.Add("/users/{id}");           
            paths.Add("/auth/me");

            string path = context.Request.Path;

            if (methods.Contains(method.ToLower()) && paths.Contains(path.ToLower()))
            {
                if (!context.User.IsInRole("Administrator"))
                {
                    context.Response.StatusCode = 401;
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
            else
            {
                await _next.Invoke(context);
            }            
        }
    }
}
