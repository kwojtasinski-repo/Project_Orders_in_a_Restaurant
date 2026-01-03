namespace Restaurant.UI.DTO
{
    public class ApiResult<T>
    {
        public T Data { get; set; }
        public ApiError Error { get; set; }
        public bool IsSuccess => Error == null;
    }

    public class ApiResult
    {
        public ApiError Error { get; set; }
        public bool IsSuccess => Error == null;
    }
}
