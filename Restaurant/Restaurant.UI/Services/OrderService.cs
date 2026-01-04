using Restaurant.Shared.DTO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Restaurant.UI.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "/api/orders";

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResult<OrderDetailsDto>> AddOrderAsync(OrderDetailsDto order)
        {
            try
            {
                var json = JsonSerializer.Serialize(order, Extensions.JsonSerializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiBaseUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var dto = JsonSerializer.Deserialize<OrderDetailsDto>(responseContent, Extensions.JsonSerializerOptions);
                    return new ApiResult<OrderDetailsDto> { Data = dto! };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent, Extensions.JsonSerializerOptions)?.Error;
                    return new ApiResult<OrderDetailsDto> { Error = error };
                }
                else
                {
                    return new ApiResult<OrderDetailsDto>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            Context = nameof(AddOrderAsync)
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResult<OrderDetailsDto>
                {
                    Error = new ApiError
                    {
                        Message = ex.Message,
                        Context = nameof(AddOrderAsync)
                    }
                };
            }
        }

        public async Task<ApiResult<List<OrderDto>>> GetAllOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApiBaseUrl);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var order = JsonSerializer.Deserialize<List<OrderDto>>(responseContent, Extensions.JsonSerializerOptions);
                    return new ApiResult<List<OrderDto>> { Data = order! };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent, Extensions.JsonSerializerOptions)?.Error;
                    return new ApiResult<List<OrderDto>> { Error = error };
                }
                else
                {
                    return new ApiResult<List<OrderDto>>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            Context = nameof(GetAllOrdersAsync)
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResult<List<OrderDto>>
                {
                    Error = new ApiError
                    {
                        Message = ex.Message,
                        Context = nameof(GetAllOrdersAsync)
                    }
                };
            }
        }

        public async Task<ApiResult<OrderDetailsDto>> GetOrderAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var order = JsonSerializer.Deserialize<OrderDetailsDto>(responseContent, Extensions.JsonSerializerOptions);
                    return new ApiResult<OrderDetailsDto> { Data = order! };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent, Extensions.JsonSerializerOptions)?.Error;
                    return new ApiResult<OrderDetailsDto> { Error = error };
                }
                else
                {
                    return new ApiResult<OrderDetailsDto>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            Context = nameof(GetOrderAsync)
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResult<OrderDetailsDto>
                {
                    Error = new ApiError
                    {
                        Message = ex.Message,
                        Context = nameof(GetOrderAsync)
                    }
                };
            }
        }

        public async Task<ApiResult> DeleteOrders(List<Guid> orderIds)
        {
            try
            {
                var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"{ApiBaseUrl}/multi")
                {
                    Content = new StringContent(JsonSerializer.Serialize(orderIds, Extensions.JsonSerializerOptions), Encoding.UTF8, "application/json")
                });
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return new ApiResult();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonSerializer.Deserialize<ApiErrorResponse>(responseContent, Extensions.JsonSerializerOptions)?.Error;
                    return new ApiResult() { Error = error };
                }
                else
                {
                    return new ApiResult
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            Context = nameof(DeleteOrders)
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
                        Context = nameof(DeleteOrders)
                    }
                };
            }
        }
    }
}
