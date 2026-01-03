using System;
using System.Text;

namespace Restaurant.Shared.DTO
{
    public class ProductSaleDto : BaseDto
    {
        public AdditionDto Addition { get; set; }
        public Guid? AdditionId { get; set; }
        public ProductDto Product { get; set; }
        public Guid ProductId { get; set; }
        public decimal EndPrice { get; set; }
        public Guid? OrderId { get; set; }
        public ProductSaleState ProductSaleState { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            var productSaleString = new StringBuilder(Product.ToString());

            if (Addition != null)
            {
                productSaleString.Append(" ");
                productSaleString.Append(Addition.ToString());
            }

            return productSaleString.ToString();
        }

    }

    public enum ProductSaleState
    {
        New, Ordered
    }
}
