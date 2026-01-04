using Restaurant.Shared.DTO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Restaurant.UI.Services
{
    internal class SettingsService : ISettingsService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "/api/settings";

        public SettingsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<SettingsDto>> GetSettings()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApiBaseUrl);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var settings = JsonSerializer.Deserialize<SettingsDto>(responseContent, Extensions.JsonSerializerOptions);
                    return new ApiResult<SettingsDto> { Data = settings! };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent, Extensions.JsonSerializerOptions)?.Error;
                    return new ApiResult<SettingsDto> { Error = error };
                }
                else
                {
                    return new ApiResult<SettingsDto>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            Context = nameof(GetSettings)
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResult<SettingsDto>
                {
                    Error = new ApiError
                    {
                        Message = ex.Message,
                        Context = nameof(GetSettings)
                    }
                };
            }
        }

        public async Task<ApiResult> SaveSettings(SettingsDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, Extensions.JsonSerializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(ApiBaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonSerializer.Deserialize<ApiErrorResponse>(await response.Content.ReadAsStringAsync(), Extensions.JsonSerializerOptions)?.Error;
                    return new ApiResult { Error = error };
                }
                else
                {
                    return new ApiResult
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            Context = nameof(SaveSettings)
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResult
                {
                    Error = new ApiError
                    {
                        Message = ex.Message,
                        Context = nameof(SaveSettings)
                    }
                };
            }
        }
    }
}
