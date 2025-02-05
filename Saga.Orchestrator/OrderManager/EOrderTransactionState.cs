namespace Saga.Orchestrator.OrderManager
{
    public enum EOrderTransactionState
    {
        NotStarted,
        BasketGot,
        BasketDeleted,
        BasketGetFailed,
        OrderCreated,
        OrderGot,
        OrderGetFailed,
        OrderCreatedFailed,
        InventoryUpdated,
        InventoryUpdateFailed,
        InventoryRollback,
        InventoryRollbackFailed,
        RollbackInventory,
        OrderDeleted,
        OrderDeletedFailed
    }
}
