namespace Restaurant.Shared.DTO
{
    public class ProductDto : BaseDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public ProductKind ProductKind { get; set; }

        public override string ToString()
        {
            return ProductName;
        }
    }
}
