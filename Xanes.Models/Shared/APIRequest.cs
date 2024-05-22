using Xanes.Utility;

namespace Xanes.Models.Shared;
public class APIRequest
{
    public HttpMethod ApiType { get; set; } = HttpMethod.Get;
    public string Url { get; set; } = null!;
    public object Data { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
    public SD.ContentType ContentType { get; set; } = SD.ContentType.Json;
}
