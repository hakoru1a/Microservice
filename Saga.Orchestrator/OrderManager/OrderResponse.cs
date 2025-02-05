namespace Saga.Orchestrator.OrderManager
{
    public class OrderResponse
    {
        private bool _success;
        public OrderResponse(bool isSuccess)
        {
            _success = isSuccess;
        }
    }
}
