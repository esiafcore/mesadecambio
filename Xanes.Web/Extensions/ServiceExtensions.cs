using Xanes.DataAccess.ServicesApi.Interface;
using Xanes.DataAccess.ServicesApi.Service;
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
    }
}

