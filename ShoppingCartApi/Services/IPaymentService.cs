using ShoppingCartApi.Models.Dto;

namespace ShoppingCartApi.Services
{
    public interface IPaymentService
    {
        Task<List<PaymentDto>> GetAllPaymentsAsync();
        Task<PaymentDto> GetPaymentByIdAsync(int paymentId);
        Task<PaymentDto> CreateUpdatePaymentAsync(PaymentDto paymentDto);
        Task<bool> DeletePaymentAsync(int paymentId);
    }
}
