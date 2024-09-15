namespace Xanes.Models.Shared;
public class ResultResponse
{
    public bool IsSuccess { get; set; } = false;
    public bool IsInfo { get; set; } = false;
    public string ErrorMessages { get; set; } = null!;
    public string SuccessMessages { get; set; } = null!;
    public object? Data { get; set; }
    public object? DataChildren { get; set; }
    public object? DataList { get; set; }
}
