using Newtonsoft.Json;
using Restaurant.Shared.DTO;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                    var settings = JsonConvert.DeserializeObject<SettingsDto>(responseContent);
                    return new ApiResult<SettingsDto> { Data = settings };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(responseContent)?.Error;
                    return new ApiResult<SettingsDto> { Error = error };
                }
                else
                {
                    return new ApiResult<SettingsDto>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            ClassObjectThrown = nameof(SettingsService),
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
                        ClassObjectThrown = nameof(SettingsService),
                        Context = nameof(GetSettings)
                    }
                };
            }
        }

        public async Task<ApiResult> SaveSettings(SettingsDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(ApiBaseUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(await response.Content.ReadAsStringAsync())?.Error;
                    return new ApiResult { Error = error };
                }
                else
                {
                    return new ApiResult
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            ClassObjectThrown = nameof(SettingsService),
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
                        ClassObjectThrown = nameof(SettingsService),
                        Context = nameof(SaveSettings)
                    }
                };
            }
        }
    }
}
