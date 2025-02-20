namespace Xanes.Web.CustomMiddleware;

public class ContentSecurityPolicyMiddleware
{
    private readonly RequestDelegate _next;

    public ContentSecurityPolicyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        string nameSecurityPolicy = "Content-Security-Policy";

        if (!context.Response.Headers.ContainsKey(nameSecurityPolicy))
        {
            context.Response.Headers.Append(nameSecurityPolicy,
                "default-src 'self' 'unsafe-inline';" +
                "connect-src 'self' 'unsafe-inline' https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 ;" +
                "font-src 'self' https://fonts.gstatic.com/ ;" +
                "object-src 'self';" +
                "style-src 'self' 'unsafe-inline' https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 https://cdnjs.cloudflare.com/ https://cdn.datatables.net/ https://cdn.jsdelivr.net/ https://fonts.googleapis.com/ ;" +
                "script-src 'self' 'wasm-unsafe-eval' 'unsafe-inline' https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 https://cdnjs.cloudflare.com/ https://cdn.datatables.net/ https://cdn.jsdelivr.net/ ;" +
                "img-src 'self' 'unsafe-inline' data: https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 http://www.w3.org ;");
        }

        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("Referrer-Policy", "no-referrer");
        context.Response.Headers.Remove("X-Powered-By");
        context.Response.Headers.Remove("Server");

        await _next(context);
    }
}