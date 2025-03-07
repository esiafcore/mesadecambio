﻿namespace Xanes.Models.Shared;

public class JsonResultResponse
{
    public JsonResultResponse()
    {
        ErrorMessages = string.Empty;
        SuccessMessages = string.Empty;
        IsSuccess = false;
        IsInfo = false;
        IsWarning = false;
        UrlRedirect = string.Empty;
    }

    public bool IsSuccess { get; set; }
    public bool IsInfo { get; set; }
    public bool IsWarning { get; set; }
    public string TitleMessages { get; set; } = null!;
    public string ErrorMessages { get; set; } = null!;
    public string SuccessMessages { get; set; } = null!;
    public string DurationTime { get; set; } = null!;
    public string? UrlRedirect { get; set; }

    public object? Data { get; set; }
}