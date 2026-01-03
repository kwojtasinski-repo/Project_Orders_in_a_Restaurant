using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Restaurant.ApplicationLogic.DTO;
using Restaurant.UI.DTO;

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
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiBaseUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var dto = JsonConvert.DeserializeObject<OrderDetailsDto>(responseContent);
                    return new ApiResult<OrderDetailsDto> { Data = dto };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(responseContent)?.Error;
                    return new ApiResult<OrderDetailsDto> { Error = error };
                }
                else
                {
                    return new ApiResult<OrderDetailsDto>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            ClassObjectThrown = nameof(OrderService),
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
                        ClassObjectThrown = nameof(OrderService),
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
                    var order = JsonConvert.DeserializeObject<List<OrderDto>>(responseContent);
                    return new ApiResult<List<OrderDto>> { Data = order };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(responseContent)?.Error;
                    return new ApiResult<List<OrderDto>> { Error = error };
                }
                else
                {
                    return new ApiResult<List<OrderDto>>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            ClassObjectThrown = nameof(OrderService),
                            Context = nameof(GetOrderAsync)
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
                        ClassObjectThrown = nameof(OrderService),
                        Context = nameof(GetOrderAsync)
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
                    var order = JsonConvert.DeserializeObject<OrderDetailsDto>(responseContent);
                    return new ApiResult<OrderDetailsDto> { Data = order };
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(responseContent)?.Error;
                    return new ApiResult<OrderDetailsDto> { Error = error };
                }
                else
                {
                    return new ApiResult<OrderDetailsDto>
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            ClassObjectThrown = nameof(OrderService),
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
                        ClassObjectThrown = nameof(OrderService),
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
                    Content = new StringContent(JsonConvert.SerializeObject(orderIds), Encoding.UTF8, "application/json")
                });
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var order = JsonConvert.DeserializeObject<OrderDetailsDto>(responseContent);
                    return new ApiResult();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = JsonConvert.DeserializeObject<ApiErrorResponse>(responseContent)?.Error;
                    return new ApiResult() { Error = error };
                }
                else
                {
                    return new ApiResult
                    {
                        Error = new ApiError
                        {
                            Message = $"Unexpected error: {response.StatusCode}",
                            ClassObjectThrown = nameof(OrderService),
                            Context = nameof(GetOrderAsync)
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
                        ClassObjectThrown = nameof(OrderService),
                        Context = nameof(GetOrderAsync)
                    }
                };
            }
        }
    }
}
