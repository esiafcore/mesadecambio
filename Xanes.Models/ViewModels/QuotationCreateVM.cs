﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Xanes.Models.ViewModels;

public class QuotationCreateVM
{

    public Quotation DataModel { get; set; } = null!;

    [ValidateNever]
    public string CurrencySourceTarget { get; set; } = string.Empty;

    [ValidateNever]
    public List<Customer> CustomerList { get; set; } = new();

    [ValidateNever]
    public IEnumerable<SelectListItem>? BankAccountSourceList { get; set; } = null;

    [ValidateNever]
    public IEnumerable<SelectListItem>? BankAccountTargetList { get; set; } = null;

    [ValidateNever]
    public List<Currency> CurrencyTransaList { get; set; } = new();

    [ValidateNever]
    public List<Currency> CurrencyDepositList { get; set; } = new();

    [ValidateNever]
    public List<Currency> CurrencyTransferList { get; set; } = new();

    [ValidateNever]
    public List<QuotationType> QuotationTypeList { get; set; } = new();

    [ValidateNever]
    public List<BusinessExecutive> BusinessExecutiveList { get; set; } = new();

    [ValidateNever]
    public DateOnly ProcessingDate { get; set; }

    [ValidateNever] 
    public bool ExistProcessingDate { get; set; } = true;

}