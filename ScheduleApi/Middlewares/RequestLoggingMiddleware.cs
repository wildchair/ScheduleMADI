using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace ScheduleApi.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var ip = context.Connection.RemoteIpAddress?.ToString();
            var method = context.Request.Method;
            var path = context.Request.Path;
            var userAgent = context.Request.Headers.UserAgent.ToString();

            _logger.LogInformation("Запрос получен: {Method} {Path} от {IP}, UA: {UserAgent}", method, path, ip, userAgent);

            try
            {
                await _next(context);
                stopwatch.Stop();

                var statusCode = context.Response.StatusCode;

                _logger.LogInformation("Запрос: {Method} {Path} от {IP}, UA: {UserAgent} => {StatusCode} за {Elapsed} мс",
                    method, path, ip, userAgent, statusCode, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex, "Ошибка при выполнении запроса: {Method} {Path} от {IP}, за {Elapsed} мс",
                    method, path, ip, stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}