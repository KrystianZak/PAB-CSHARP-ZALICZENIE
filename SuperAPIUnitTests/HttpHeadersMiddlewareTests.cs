using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SuperAPITests
{
    public class HttpHeadersMiddlewareTests
    {
        [Fact]
        public async Task Middleware_Adds_Custom_Headers()
        {
            // Arrange
            var webHostBuilder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseMiddleware<SuperAPI.Middleware.HttpHeadersMiddleware>();
                    app.Run(async context =>
                    {
                        await context.Response.WriteAsync("Hello, World!");
                    });
                });

            var testServer = new TestServer(webHostBuilder);
            var client = testServer.CreateClient();

            // Act
            var response = await client.GetAsync("/");
            var responseHeaders = response.Headers;

            // Assert
            Assert.True(responseHeaders.Contains("X-Custom-Response-Header"));
            Assert.Equal("ResponseHeaderValue", responseHeaders.GetValues("X-Custom-Response-Header").First());
        }
    }
}
