using System.Net;
using System.Text.Json;
using Restaurant.Shared.DTO;

namespace Restaurant.UI.Services
{
    public class MenuService : IMenuService
    {
        private readonly HttpClient _httpClient;

        public MenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<MenuDto>> GetMenuAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/menu");
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var menu = JsonSerializer.Deserialize<MenuDto>(content, Extensions.JsonSerializerOptions);
                    return new ApiResult<MenuDto> { Data = menu ?? new MenuDto() };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonSerializer.Deserialize<ApiErrorResponse>(content, Extensions.JsonSerializerOptions)?.Error;
                    return new ApiResult<MenuDto> { Error = error };
                }
                else
                {
                    return new ApiResult<MenuDto>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            Context = nameof(GetMenuAsync)
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResult<MenuDto>
                {
                    Error = new ApiError
                    {
                        Message = ex.Message,
                        Context = nameof(GetMenuAsync)
                    }
                };
            }
        }
    }
}
