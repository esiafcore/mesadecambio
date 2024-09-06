using Xanes.DataAccess.ServicesApi.Interface.eSiafN4;
using Xanes.DataAccess.ServicesApi.Interface.XanesN4;
using Xanes.DataAccess.ServicesApi.Interface.XanesN8;
using Xanes.DataAccess.ServicesApi.Service.eSiafN4;
using Xanes.DataAccess.ServicesApi.Service.XanesN4;
using Xanes.DataAccess.ServicesApi.Service.XanesN8;
using Xanes.LoggerService;

namespace Xanes.Web.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureHttpClientService(this IServiceCollection services)
    {
        //Authentication and Authorization
        services.AddHttpClient<IQuotationLegacyService, QuotationLegacyService>();
        services.AddTransient<IQuotationLegacyService, QuotationLegacyService>();

        services.AddHttpClient<IQuotationDetailLegacyService, QuotationDetailLegacyService>();
        services.AddTransient<IQuotationDetailLegacyService, QuotationDetailLegacyService>();

        services.AddHttpClient<ICustomerLegacyService, CustomerLegacyService>();
        services.AddTransient<ICustomerLegacyService, CustomerLegacyService>();

        services.AddHttpClient<IAuthService, AuthService>();
        services.AddTransient<IAuthService, AuthService>();

        services.AddHttpClient<ITransaccionBcoService, TransaccionBcoService>();
        services.AddTransient<ITransaccionBcoService, TransaccionBcoService>();

        services.AddHttpClient<ITransaccionBcoDetalleService, TransaccionBcoDetalleService>();
        services.AddTransient<ITransaccionBcoDetalleService, TransaccionBcoDetalleService>();
        
        services.AddHttpClient<IConfigBcoService, ConfigBcoService>();
        services.AddTransient<IConfigBcoService, ConfigBcoService>();

        services.AddHttpClient<IConsecutivoBcoService, ConsecutivoBcoService>();
        services.AddTransient<IConsecutivoBcoService, ConsecutivoBcoService>();

        services.AddHttpClient<IConsecutivoBcoDetalleService, ConsecutivoBcoDetalleService>();
        services.AddTransient<IConsecutivoBcoDetalleService, ConsecutivoBcoDetalleService>();
    }
}

