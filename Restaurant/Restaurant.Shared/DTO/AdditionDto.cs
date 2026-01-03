namespace Restaurant.Shared.DTO
{
    public class AdditionDto : BaseDto
    {
        public string AdditionName { get; set; }
        public decimal Price { get; set; }
        public ProductKind ProductKind { get; set; }

        public override string ToString()
        {
            return AdditionName;
        }
    }
}
