﻿
namespace Xanes.DataAccess.ServicesApi.Interface;
public interface IQuotationDetailLegacyService
{
    Task<string> GetAllLegacyAsync(string token, int pageSize, int pageNumber, DateOnly beginDate, DateOnly endDate, string? identificationNumber);
}

