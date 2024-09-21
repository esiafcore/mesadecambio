namespace Xanes.Web.CustomMiddleware;

public class ContentSecurityPolicyMiddlewareDevelopment
{
    private readonly RequestDelegate _next;

    public ContentSecurityPolicyMiddlewareDevelopment(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string nameSecurityPolicy = "Content-Security-Policy-Report-Only";

        if (!context.Response.Headers.ContainsKey(nameSecurityPolicy))
        {
            context.Response.Headers.Add(nameSecurityPolicy,
                "default-src 'self' 'unsafe-inline';" +
                "connect-src 'self' 'unsafe-inline' ws://localhost:37009 http://localhost:37009 wss://localhost:44393 wss://localhost:1528 wss://localhost:14996 wss://localhost:44352 ws://localhost:41161 http://localhost:41161 wss://localhost:44344 http://localhost:48367 http://localhost:62224 https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 http://localhost:58560 wss://localhost:44360 wss://localhost:48367 ws://localhost:48367 ws://localhost:44360 ws://localhost:58560 wss://localhost:44351 wss://localhost:44390 ws://localhost:62224 wss://localhost:44331 ws://localhost:50059 http://localhost:50059 wss://localhost:44374 http://localhost:50087 ws://localhost:50087 wss://localhost:44334 http://localhost:55127 ws://localhost:55127 wss://localhost:44345 wss://localhost:44346 http://localhost:50447 ws://localhost:50447;" +
                "font-src 'self' https://fonts.gstatic.com/ ;" +
                "object-src 'self';" +
                "style-src 'self' 'unsafe-inline' https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 https://localhost https://cdnjs.cloudflare.com/ https://cdn.datatables.net/ https://cdn.jsdelivr.net/ https://fonts.googleapis.com/ ;" +
                "script-src 'self' 'wasm-unsafe-eval' 'unsafe-inline' https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 https://localhost https://cdnjs.cloudflare.com/ https://cdn.datatables.net/ https://cdn.jsdelivr.net/ ;" +
                "img-src 'self' 'unsafe-inline' data: https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 https://localhost http://www.w3.org ;");
        }

        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("Referrer-Policy", "no-referrer");

        await _next(context);
    }
}