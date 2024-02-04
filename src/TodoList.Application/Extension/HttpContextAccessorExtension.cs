using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TodoList.Application.Extension;

public static class HttpContextAccessorExtension
{
    public static int? GetUserId(this IHttpContextAccessor? contextAccessor)
    {
        var id = contextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return id == null ? null : int.Parse(id);
    }
}