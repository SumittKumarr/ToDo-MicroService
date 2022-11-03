using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace ToDoService.NewFolder
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public MyMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;

            
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string token = httpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ")[1];
            

            
            string _apiUrl = "https://localhost:7290/IsTokenValid";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var responseMessage = await client.GetAsync(_apiUrl);
                string userId = responseMessage.Content.ReadAsStringAsync().Result;
                httpContext.Items["userId"] = userId;
                if(userId == null)
                {
                    return;
                }
            }
           



            await _next(httpContext); // calling next middleware

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }







}
