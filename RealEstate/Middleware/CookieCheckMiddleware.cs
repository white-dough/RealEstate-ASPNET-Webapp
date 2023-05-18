namespace RealEstate.Middleware
{
    public class CookieCheckMiddleware
    {
        private readonly RequestDelegate _next;
        public CookieCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies["RealEstateCookie"] == null && httpContext.Request.Path != "/Login")
            {
                httpContext.Response.Redirect("/Login");
            }
            await _next(httpContext);
        }
    }

    public static class CookieCheckMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieCheckMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieCheckMiddleware>();
        }
    }
}
