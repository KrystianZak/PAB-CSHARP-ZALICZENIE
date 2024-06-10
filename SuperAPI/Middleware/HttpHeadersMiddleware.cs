using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SuperAPI.Middleware
{
    public class HttpHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Przykład dodania nagłówka do requestu
            context.Request.Headers.Add("X-Custom-Request-Header", "RequestHeaderValue");

            // Przykład dodania nagłówka do response
            context.Response.Headers.Add("X-Custom-Response-Header", "ResponseHeaderValue");

            await _next(context);
        }
    }
}
