using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace Xanes.Models.ViewModels;

public class ProcessingDateVM
{
    public DateOnly ProcessingDate { get; set; }

}