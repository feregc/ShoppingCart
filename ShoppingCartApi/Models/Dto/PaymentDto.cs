namespace ShoppingCartApi.Models.Dto
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public float Amount { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}
