using AutoMapper;
using Constracts.Saga.OrderManager;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Basket;
using Shared.DTOs.Inventory;
using Shared.DTOs.Order;
using Shared.Enums.Order;

namespace Saga.Orchestrator.OrderManager
{
    public class SagaOrderManager : ISagaOrderManager<BasketCheckoutDto,OrderResponse>
    {
        private IOrderHttpRepository _orderHttpRepository;
        private IBasketHttpRepository _basketHttpRepository;
        private IInventoryHttpRepository _inventoryHttpRepository;
        private IMapper _mapper;
        private Serilog.ILogger _logger;


        public SagaOrderManager(IOrderHttpRepository orderHttpRepository, IBasketHttpRepository basketHttpRepository,
                   IInventoryHttpRepository inventoryHttpRepository, IMapper mapper, Serilog.ILogger logger)
        {
            _orderHttpRepository = orderHttpRepository;
            _basketHttpRepository = basketHttpRepository;
            _mapper = mapper;
            _logger = logger;
            _inventoryHttpRepository = inventoryHttpRepository;
        }

        public OrderResponse CreateOrder(BasketCheckoutDto input)
        {
            var orderStateMachine = new Stateless.StateMachine<EOrderTransactionState, EOrderAction>
                                                                    (EOrderTransactionState.NotStarted);

            long orderId = -1;
            CartDto cart = null;
            OrderDto addedOrder = null;
            string? inventoryDocumentNo = null;
            orderStateMachine.Configure(EOrderTransactionState.NotStarted)
                .PermitDynamic( EOrderAction.GetBasket, destinationStateSelector: () =>
                {
                    cart = _basketHttpRepository.GetBasket(input.UserName).Result;
                    return cart != null ? EOrderTransactionState.BasketGot : EOrderTransactionState.BasketGetFailed;
                });

            orderStateMachine.Configure(EOrderTransactionState.BasketGot)
               .PermitDynamic(EOrderAction.CreateOrder, destinationStateSelector: () =>
               {
                   var order = _mapper.Map<CreateOrderDto>(input);
                   order.Status = order.Status ?? EOrderStatus.Paid.ToString();
                   order.TotalPrice = cart.TotalPrice;
                   orderId = _orderHttpRepository.CreateOrder(order).Result;
                   return orderId > 0 ? EOrderTransactionState.OrderCreated : EOrderTransactionState.OrderCreatedFailed;
               }).OnEntry(() => orderStateMachine.Fire(EOrderAction.CreateOrder));

            orderStateMachine.Configure(EOrderTransactionState.OrderCreated)
               .PermitDynamic(trigger: EOrderAction.GetOrder, destinationStateSelector: () =>
               {
                   addedOrder = _orderHttpRepository.GetOrder(orderId).Result;
                   return addedOrder != null ? EOrderTransactionState.OrderGot : EOrderTransactionState.OrderGetFailed;
               })
             .OnEntry(() => orderStateMachine.Fire(EOrderAction.GetOrder));

            orderStateMachine.Configure(EOrderTransactionState.OrderGot)
               .PermitDynamic(trigger: EOrderAction.UpdateInventory, destinationStateSelector: () =>
               {
                   var salesOrder = new SalesOrderDto
                   {
                       OrderNo = addedOrder.No,
                       SaleItems = _mapper.Map<List<SaleItemDto>>(cart.Items)
                   };
                   inventoryDocumentNo = _inventoryHttpRepository.CreateOrderSale(addedOrder.No, salesOrder).Result;

                   return inventoryDocumentNo != null ? EOrderTransactionState.InventoryUpdated : EOrderTransactionState.InventoryUpdateFailed;
               })
               .OnEntry(() => orderStateMachine.Fire(EOrderAction.UpdateInventory));

            orderStateMachine.Configure(EOrderTransactionState.InventoryUpdated)
                    .PermitDynamic(trigger: EOrderAction.DeleteBasket, destinationStateSelector: () =>
                    {
                        var result = _basketHttpRepository.DeleteBasket(input.UserName).Result;
                        return result ? EOrderTransactionState.BasketDeleted : EOrderTransactionState.InventoryUpdateFailed;
                    })
                    .OnEntry(entryAction: () => orderStateMachine.Fire(trigger: EOrderAction.DeleteBasket));
                
            orderStateMachine.Configure(EOrderTransactionState.InventoryUpdateFailed)
               .PermitDynamic(trigger: EOrderAction.DeleteInventory, destinationStateSelector: () =>
               {
                   RollbackOrder(input.UserName, inventoryDocumentNo, orderId);
                   return EOrderTransactionState.InventoryRollback;
               }).OnEntry(entryAction: () => orderStateMachine.Fire(trigger: EOrderAction.DeleteInventory)); ;

            orderStateMachine.Fire(EOrderAction.GetBasket);

            return new OrderResponse(orderStateMachine.State == EOrderTransactionState.InventoryUpdated);
        }


        public OrderResponse RollbackOrder(string username, string documentNo, long orderId)
        {
            var orderStateMachine =
                    new Stateless.StateMachine<EOrderTransactionState, EOrderAction>(EOrderTransactionState.RollbackInventory);

            orderStateMachine.Configure(EOrderTransactionState.RollbackInventory)
               .PermitDynamic(trigger: EOrderAction.DeleteInventory, destinationStateSelector: () =>
               {
                   _inventoryHttpRepository.DeleteOrderByDocumentNo(documentNo);
                   return EOrderTransactionState.InventoryRollback;
               });

            orderStateMachine.Configure(EOrderTransactionState.InventoryRollback)
               .PermitDynamic(trigger: EOrderAction.DeleteOrder, destinationStateSelector: () =>
               {
                   var result = _orderHttpRepository.DeleteOrder(orderId).Result;
                   return result ? EOrderTransactionState.OrderDeleted : EOrderTransactionState.OrderDeletedFailed;
               }).OnEntry(entryAction: () => orderStateMachine.Fire(trigger: EOrderAction.DeleteOrder));

            orderStateMachine.Fire(trigger: EOrderAction.DeleteInventory);


            return new OrderResponse(orderStateMachine.State == EOrderTransactionState.InventoryRollback);
        }
    }
}
