namespace Xanes.Models.Shared;

public class JsonResultResponse
{
    public JsonResultResponse()
    {
        ErrorMessages = string.Empty;
        SuccessMessages = string.Empty;
        IsSuccess = false;
        UrlRedirect = string.Empty;
    }
    public bool IsSuccess { get; set; }
    public string ErrorMessages { get; set; }
    public string SuccessMessages { get; set; }
    public string DurationTime { get; set; } = null!;
    public string? UrlRedirect { get; set; }

    public object? Data { get; set; }
}