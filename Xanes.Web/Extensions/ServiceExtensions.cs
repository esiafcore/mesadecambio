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
    public static void ConfigureLoggerService(this IServiceCollection srvs) =>
        srvs.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureHttpClientService(this IServiceCollection srvs)
    {
        //Authentication and Authorization
        srvs.AddHttpClient<IQuotationLegacyService, QuotationLegacyService>();
        srvs.AddTransient<IQuotationLegacyService, QuotationLegacyService>();

        srvs.AddHttpClient<IQuotationDetailLegacyService, QuotationDetailLegacyService>();
        srvs.AddTransient<IQuotationDetailLegacyService, QuotationDetailLegacyService>();

        srvs.AddHttpClient<ICustomerLegacyService, CustomerLegacyService>();
        srvs.AddTransient<ICustomerLegacyService, CustomerLegacyService>();

        srvs.AddHttpClient<IAuthService, AuthService>();
        srvs.AddTransient<IAuthService, AuthService>();
        
        srvs.AddHttpClient<ICuentaBancariaService, CuentaBancariaService>();
        srvs.AddTransient<ICuentaBancariaService, CuentaBancariaService>();

        srvs.AddHttpClient<IBancoService, BancoService>();
        srvs.AddTransient<IBancoService, BancoService>();

        srvs.AddHttpClient<ITransaccionBcoService, TransaccionBcoService>();
        srvs.AddTransient<ITransaccionBcoService, TransaccionBcoService>();

        srvs.AddHttpClient<ITransaccionBcoDetalleService, TransaccionBcoDetalleService>();
        srvs.AddTransient<ITransaccionBcoDetalleService, TransaccionBcoDetalleService>();
        
        srvs.AddHttpClient<IConfigBcoService, ConfigBcoService>();
        srvs.AddTransient<IConfigBcoService, ConfigBcoService>();

        srvs.AddHttpClient<IConsecutivoBcoService, ConsecutivoBcoService>();
        srvs.AddTransient<IConsecutivoBcoService, ConsecutivoBcoService>();

        srvs.AddHttpClient<IConsecutivoBcoDetalleService, ConsecutivoBcoDetalleService>();
        srvs.AddTransient<IConsecutivoBcoDetalleService, ConsecutivoBcoDetalleService>();

        srvs.AddHttpClient<IAsientoContableService, AsientoContableService>();
        srvs.AddTransient<IAsientoContableService, AsientoContableService>();

        srvs.AddHttpClient<IAsientoContableDetalleService, AsientoContableDetalleService>();
        srvs.AddTransient<IAsientoContableDetalleService, AsientoContableDetalleService>();

        srvs.AddHttpClient<IConfigCntService, ConfigCntService>();
        srvs.AddTransient<IConfigCntService, ConfigCntService>();

        srvs.AddHttpClient<IConsecutivoCntService, ConsecutivoCntService>();
        srvs.AddTransient<IConsecutivoCntService, ConsecutivoCntService>();

        srvs.AddHttpClient<IConsecutivoCntDetalleService, ConsecutivoCntDetalleService>();
        srvs.AddTransient<IConsecutivoCntDetalleService, ConsecutivoCntDetalleService>();

    }
}

