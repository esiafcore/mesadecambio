using static Stimulsoft.Report.Help.StiHelpProvider;
using static Stimulsoft.Report.StiOptions.Export.Pdf;

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
        string nameSecurityPolicy = "Content-Security-Policy-Report-Only";
        //string nameSecurityPolicy = "Content-Security-Policy";

        if (!context.Response.Headers.ContainsKey(nameSecurityPolicy))
        {
            context.Response.Headers.Add(nameSecurityPolicy,
                "default-src 'self' 'unsafe-inline';" +
                "connect-src 'self' 'unsafe-inline' http://localhost:62224 https://209.145.54.249:7201 https://vmi531999.contaboserver.net:7201 http://localhost:58560 ws://localhost:58560 wss://localhost:44351 wss://localhost:44390 ws://localhost:62224 wss://localhost:44331;" +
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
        //context.Response.Headers.Add("Permissions-Policy", "camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), usb=()");
        //context.Response.Headers.Remove("X-Powered-By");
        //context.Response.Headers.Remove("Server");

        await _next(context);
    }
}