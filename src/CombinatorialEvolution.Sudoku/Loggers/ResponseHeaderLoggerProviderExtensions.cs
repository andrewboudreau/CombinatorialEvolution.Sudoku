namespace CombinatorialEvolution.Sudoku.Loggers
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public static class ResponseHeaderLoggerProviderExtensions
    {
        public static ILoggerFactory AddResponseHeaderLogger(this ILoggerFactory loggerFactory, IApplicationBuilder app)
        {
            //app.UseMiddleware<ResponseHeaderLoggerMiddleware>();
            loggerFactory.AddProvider(new ResponseHeaderLoggerProvider());

            return loggerFactory;
        }

        public class ResponseHeaderLoggerMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ResponseHeaderLogger> _logger;

            public ResponseHeaderLoggerMiddleware(RequestDelegate next, ILogger<ResponseHeaderLogger> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task Invoke(HttpContext context)
            {
                var startTime = DateTime.UtcNow;

                var watch = Stopwatch.StartNew();
                await _next.Invoke(context);
                watch.Stop();

                var logTemplate = @"
Client IP: {clientIP}
Request path: {requestPath}
Request content type: {requestContentType}
Request content length: {requestContentLength}
Start time: {startTime}
Duration: {duration}";

                _logger.LogInformation(logTemplate,
                    context.Connection.RemoteIpAddress.ToString(),
                    context.Request.Path,
                    context.Request.ContentType,
                    context.Request.ContentLength,
                    startTime,
                    watch.ElapsedMilliseconds);
            }
        }
    }
}
