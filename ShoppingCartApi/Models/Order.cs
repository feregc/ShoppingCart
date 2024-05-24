namespace ShoppingCartApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
