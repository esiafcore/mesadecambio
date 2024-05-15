using static Stimulsoft.Report.Help.StiHelpProvider;
using static Stimulsoft.Report.StiOptions.Export.Pdf;

namespace Xanes.Web.CustomMiddleware;

public class ContentSecurityPolicyMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    public ContentSecurityPolicyMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext context)
    {
        string nameSecurityPolicy = "Content-Security-Policy-Report-Only";
        //string nameSecurityPolicy = "Content-Security-Policy";

        if (!context.Response.Headers.ContainsKey(nameSecurityPolicy))
        {
            context.Response.Headers.Add(nameSecurityPolicy,
                "default-src 'self' 'unsafe-inline';" +
                "connect-src 'self' 'unsafe-inline' https://vmi531999.contaboserver.net:7201 http://localhost:58560 ws://localhost:58560 wss://localhost:44351;" +
                "font-src 'self' https://fonts.gstatic.com/ ;" +
                "object-src 'self';" +
                "style-src 'self' 'unsafe-inline' https://vmi531999.contaboserver.net:7201 https://localhost https://cdnjs.cloudflare.com/ https://cdn.datatables.net/ https://cdn.jsdelivr.net/ https://fonts.googleapis.com/ ;" +
                "script-src 'self' 'wasm-unsafe-eval' 'unsafe-inline' https://vmi531999.contaboserver.net:7201 https://localhost https://cdnjs.cloudflare.com/ https://cdn.datatables.net/ https://cdn.jsdelivr.net/ ;" +
                "img-src 'self' 'unsafe-inline' data: https://vmi531999.contaboserver.net:7201 https://localhost http://www.w3.org ;");
        }

        await _requestDelegate(context);
    }
}