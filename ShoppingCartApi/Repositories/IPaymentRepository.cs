using ShoppingCartApi.Models;

namespace ShoppingCartApi.Repositories
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<Payment> CreateUpdateAsync(Payment payment);
        Task<bool> DeletePaymentAsync(int paymentId);
    }
}
