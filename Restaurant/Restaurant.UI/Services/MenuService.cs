using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Restaurant.UI.DTO;

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
                    var menu = JsonConvert.DeserializeObject<MenuDto>(content);
                    return new ApiResult<MenuDto> { Data = menu };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(content)?.Error;
                    return new ApiResult<MenuDto> { Error = error };
                }
                else
                {
                    return new ApiResult<MenuDto>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            ClassObjectThrown = nameof(MenuService),
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
                        ClassObjectThrown = nameof(MenuService),
                        Context = nameof(GetMenuAsync)
                    }
                };
            }
        }
    }
}
