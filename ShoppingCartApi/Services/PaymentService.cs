using AutoMapper;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService (IPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<PaymentDto> CreateUpdatePaymentAsync(PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            var updatedPayment = await _paymentRepository.CreateUpdateAsync(payment);
            return _mapper.Map<PaymentDto>(updatedPayment);

        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            return await _paymentRepository.DeletePaymentAsync(paymentId);
        }

        public async Task<List<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllPaymentsAsync();
            return _mapper.Map<List<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            return _mapper.Map<PaymentDto>(payment);
        }
    }
}
