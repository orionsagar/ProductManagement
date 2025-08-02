using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure;
using System.Security.Claims;

namespace ProductManagement.Application.Middleware
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        //{
        //    var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
        //    var path = context.Request.Path;
        //    var method = context.Request.Method;

        //    // Optionally: Read request body here for detailed info (complex, be cautious)

        //    // Call next middleware
        //    await _next(context);

        //    // Log after response
        //    var actionDescription = $"{method} {path}";

        //    var log = new AuditLog
        //    {
        //        UserId = userId,
        //        Action = actionDescription,
        //        Timestamp = DateTime.UtcNow
        //    };

        //    dbContext.AuditLogs.Add(log);
        //    await dbContext.SaveChangesAsync();
        //}

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var path = context.Request.Path;
            var method = context.Request.Method;

            // Wait for the next middleware (process request)
            await _next(context);

            // Check if the request was aborted/canceled
            if (context.RequestAborted.IsCancellationRequested)
            {
                // Optionally log or just skip audit log save
                return;
            }

            var actionDescription = $"{method} {path}";

            var log = new AuditLog
            {
                UserId = userId,
                UserName = context.User?.Identity?.Name ?? "Unknown",
                EntityName = "HttpRequest",
                EntityId = context.User.Identity.GetUserId() ?? "1", // No specific entity ID for HTTP requests
                Action = actionDescription,
                Timestamp = DateTime.UtcNow
            };

            // Pass the cancellation token to EF Core SaveChangesAsync
            await dbContext.AuditLogs.AddAsync(log, context.RequestAborted);
            await dbContext.SaveChangesAsync(context.RequestAborted);
        }

    }


}
