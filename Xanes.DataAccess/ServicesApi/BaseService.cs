using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using Xanes.Models.Shared;
using Xanes.Utility;
using System.Text.Json;


namespace Xanes.DataAccess.ServicesApi;
public class BaseService : IBaseService
{
    private readonly IConfiguration _config;
    public APIResponse ResponseModel { get; set; }

    public IHttpClientFactory httpClient { get; set; }
    public BaseService(IHttpClientFactory httpClient, IConfiguration config)
    {
        _config = config;
        this.ResponseModel = new();
        this.httpClient = httpClient;
    }

    public async Task<string> SendAsync(APIRequest apiRequest, Pagination? pagination = null)
    {
        StringBuilder errorsMessagesBuilder = new StringBuilder();

        try
        {
            var client = httpClient.CreateClient("Api");
            int timeoutClient = (int)Convert.ToInt32(_config["ServicesUrl:TimeOutClient"]);
            client.Timeout = TimeSpan.FromSeconds(timeoutClient);
            HttpRequestMessage message = new HttpRequestMessage();

            //Agregado para funcionalidad de Upload File P01
            if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
            {
                message.Headers.Add("Accept", "*/*");
            }
            else
            {
                message.Headers.Add("Accept", "application/json");
            }


            message.RequestUri = new Uri(apiRequest.Url);

            //Agregado para funcionalidad de Upload File P01
            if (apiRequest.ContentType == SD.ContentType.MultipartFormData)
            {
                var content = new MultipartFormDataContent();
                foreach (var prop in apiRequest.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(apiRequest.Data);
                    if (value is FormFile)
                    {
                        var file = (FormFile)value;
                        if (file != null)
                        {
                            content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                        }
                    }
                    else
                    {
                        content.Add(new StringContent(value != null ? value.ToString() : string.Empty), prop.Name);
                    }
                }

                message.Content = content;
            }
            else
            {
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonSerializer.Serialize(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
            }

            //Si hay token, pasarlo
            //====================
            if (!string.IsNullOrEmpty(apiRequest.Token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
            }

            message.Method = apiRequest.ApiType;

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);
            if (pagination != null)
            {
                var paginationHeader = JsonSerializer.Deserialize<Pagination>(apiResponse.Headers.GetValues(AC.PaginationHeaderName).First());
                pagination.pageNumber = paginationHeader.pageNumber;
                pagination.totalPages = paginationHeader.totalPages;
                pagination.totalCount = paginationHeader.totalCount;
                pagination.pageSize = paginationHeader.pageSize;
            }

            var apiContent = await apiResponse.Content.ReadAsStringAsync();

            try
            {
                //APIResponse ApiResponse = JsonSerializer.Deserialize<APIResponse>(apiContent);
                //if (ApiResponse != null && (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest || apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound))
                //{
                //    ApiResponse.statusCode = System.Net.HttpStatusCode.BadRequest;
                //    ApiResponse.isSuccess = false;
                //    var res = JsonSerializer.Serialize(ApiResponse);
                //    return res;
                //}
            }
            catch (Exception)
            {
                return apiContent;
            }

            return apiContent;
        }
        catch (HttpRequestException e)
        {
            var dto = new APIResponse
            {
                errorMessages = new List<string> { Convert.ToString(e.Message) },
                isSuccess = false
            };
            var res = JsonSerializer.Serialize(dto);
            return res;
        }
        catch (Exception e)
        {
            var dto = new APIResponse
            {
                errorMessages = new List<string> { Convert.ToString(e.Message) },
                isSuccess = false
            };
            var res = JsonSerializer.Serialize(dto);
            return res;
        }
    }
}

