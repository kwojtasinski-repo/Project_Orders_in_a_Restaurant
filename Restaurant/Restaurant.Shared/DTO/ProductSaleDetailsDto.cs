namespace Restaurant.Shared.DTO
{
    public class ProductSaleDetailsDto : ProductSaleDto
    {
        public OrderDto Order { get; set; }
    }
}
