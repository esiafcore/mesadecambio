using System.Net;

namespace Xanes.Models.Shared;
public class APIResponse
{
    public HttpStatusCode statusCode { get; set; }
    public bool isSuccess { get; set; } = true;
    public List<string> errorMessages { get; set; } = new List<string>();
    public List<string> successMessages { get; set; } = new List<string>();
    public object? result { get; set; }
}
