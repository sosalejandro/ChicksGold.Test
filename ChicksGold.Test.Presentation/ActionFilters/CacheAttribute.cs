   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.Filters;
   using Microsoft.Extensions.Caching.Memory;
   using System;
   using System.Security.Cryptography;
   using System.Text;
   using System.Text.Json;

public class CacheAttribute : ActionFilterAttribute
{
    private readonly IMemoryCache _cache;
    private readonly int _duration;

    public CacheAttribute(IMemoryCache cache, int duration = 60)
    {
        _cache = cache;
        _duration = duration;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var cacheKey = GenerateCacheKeyFromRequest(context);
        if (_cache.TryGetValue(cacheKey, out var cachedResponse))
        {
            context.Result = new JsonResult(cachedResponse);
            return;
        }
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is OkObjectResult okResult)
        {
            var cacheKey = GenerateCacheKeyFromExecutedContext(context);
            _cache.Set(cacheKey, okResult.Value, TimeSpan.FromSeconds(_duration));
        }
    }

    private string GenerateCacheKeyFromRequest(ActionExecutingContext context)
    {
        var requestBody = JsonSerializer.Serialize(context.ActionArguments);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(requestBody));
        return Convert.ToBase64String(hash);
    }

    private string GenerateCacheKeyFromExecutedContext(ActionExecutedContext context)
    {
        var requestBody = JsonSerializer.Serialize(context.ActionDescriptor);
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(requestBody));
        return Convert.ToBase64String(hash);
    }
}
   