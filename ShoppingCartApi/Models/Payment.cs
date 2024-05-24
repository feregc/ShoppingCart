namespace ShoppingCartApi.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public float Amount { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}
