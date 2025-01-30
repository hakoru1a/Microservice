using AutoMapper;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Basket;
using Serilog;
using Shared.DTOs.Inventory;
using Shared.DTOs.Order;
using Shared.Enums.Order;
namespace Saga.Orchestrator.Services
{
    public class CheckoutService : ICheckoutService
    {
        private IOrderHttpRepository _orderHttpRepository;
        private IBasketHttpRepository _basketHttpRepository;
        private IInventoryHttpRepository _inventoryHttpRepository;
        private IMapper _mapper;
        private Serilog.ILogger _logger;

        public CheckoutService(IOrderHttpRepository orderHttpRepository, IBasketHttpRepository basketHttpRepository,
            IInventoryHttpRepository inventoryHttpRepository, IMapper mapper, Serilog.ILogger logger)
        {
            _orderHttpRepository = orderHttpRepository;
            _basketHttpRepository = basketHttpRepository;
            _mapper = mapper;
            _logger = logger;
            _inventoryHttpRepository = inventoryHttpRepository;
        }
        public async Task<bool> CheckoutOrder(string username, BasketCheckoutDto basketCheckout)
        {
            // Get cart from BasketHttpRepository
            _logger.Information(messageTemplate: $"Start: Get Cart {username}");

            var cart = await _basketHttpRepository.GetBasket(username);
            if (cart == null) return false;
            _logger.Information(messageTemplate: $"End: Get Cart {username} success");

            // Create Order from OrderHttpRepository
            _logger.Information(messageTemplate: $"Start: Create Order");

            var order = _mapper.Map<CreateOrderDto>(basketCheckout);
            order.TotalPrice = cart.TotalPrice;
            order.Status = EOrderStatus.Paid.ToString();
            var orderId = await _orderHttpRepository.CreateOrder(order);
            if (orderId < 0) return false;
            var addedOrder = await _orderHttpRepository.GetOrder(orderId);
            _logger.Information(messageTemplate: $"End: Created Order success, Order Id: {orderId} - Document No - {addedOrder.No}");

            var inventoryDocumentNos = new List<string>();
            bool result;
            try
            {
                foreach (var item in cart.Items)
                {
                    _logger.Information(messageTemplate: $"Start: Sale Item No: {item.No} - Quantity: {item.Quantity}");

                    var saleOrder = new SalesProductDto(addedOrder.No.ToString(), item.Quantity);
                    saleOrder.SetItemNo(item.No);
                    var documentNo = await _inventoryHttpRepository.CreateSalesOrder(saleOrder);
                    inventoryDocumentNos.Add(documentNo);

                    _logger.Information(messageTemplate: $"End: Sale Item No: {item.No} - Quantity: {item.Quantity} - Doc");
                }

                result = await _basketHttpRepository.DeleteBasket(username);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                await RollbackCheckoutOrder(username, orderId, inventoryDocumentNos);
                result = false;
            }
            return result;
        }

        private async Task RollbackCheckoutOrder(string username, long orderId, List<string> inventoryDocumentNos)
        {
            _logger.Information(messageTemplate: $"Start: RollbackCheckoutOrder for username: {username}, " +
                                              $"order id: {orderId}, " +
                                              $"inventory document nos: {String.Join(", ", inventoryDocumentNos)}");

            await _orderHttpRepository.DeleteOrder(orderId);
            var deletedDocumentNos = new List<string>();
            // delete order by order's id, order's document no
            foreach (var documentNo in inventoryDocumentNos)
            {
                await _inventoryHttpRepository.DeleteOrderByDocumentNo(documentNo);
                deletedDocumentNos.Add(documentNo);
            }
            _logger.Information(messageTemplate: $"End: Deleted Inventory Document Nos: ");
        }

    }
}
