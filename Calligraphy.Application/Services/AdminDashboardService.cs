using Calligraphy.Application.Interfaces.Repositories;

namespace Calligraphy.Application.Services;

public class AdminDashboardService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentRepository _paymentRepository;

    public AdminDashboardService(
        IProductRepository productRepository,
        IUserRepository userRepository,
        IOrderRepository orderRepository,
        IPaymentRepository paymentRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<object> GetDashboardAsync()
    {
        var totalProducts =
            (await _productRepository.GetAllAsync()).Count;

        var totalUsers =
            (await _userRepository.GetAllAsync()).Count;

        var totalOrders =
            await _orderRepository.GetOrderCountAsync();

        var pendingOrders =
            await _orderRepository.GetPendingOrderCountAsync();

        var totalPayments =
            await _paymentRepository.GetAllPaymentsAsync();

        var successfulPayments =
            totalPayments.Count(p => p.Status == "Success");

        var totalRevenue =
            totalPayments
                .Where(p => p.Status == "Success")
                .Sum(p => p.Amount);

        return new
        {
            TotalProducts = totalProducts,
            TotalUsers = totalUsers,
            TotalOrders = totalOrders,
            PendingOrders = pendingOrders,
            TotalPayments = totalPayments.Count,
            SuccessfulPayments = successfulPayments,
            TotalRevenue = totalRevenue
        };
    }
}