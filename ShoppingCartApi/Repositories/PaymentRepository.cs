using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Data;
using ShoppingCartApi.Models;

namespace ShoppingCartApi.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Payment> CreateUpdateAsync(Payment payment)
        {
            if (payment.Id > 0)
            {
                _context.Payments.Update(payment);
            }
            else
            {
                await _context.Payments.AddAsync(payment);
            }
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(paymentId);
                if (payment == null)
                {
                    return false;
                }
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments.FindAsync(paymentId);
        }
    }
}
