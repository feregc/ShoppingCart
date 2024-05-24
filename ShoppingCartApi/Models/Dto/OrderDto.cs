namespace ShoppingCartApi.Models.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
