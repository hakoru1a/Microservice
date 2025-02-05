using Grpc.Core;
using Inventory.gRPC.Protos;
using Polly;
using Polly.Retry;
using Serilog;
using ILogger = Serilog.ILogger;
namespace Basket.API.gRPCServices
{
    public class StockItemGrpcService
    {
        private StockProtoService.StockProtoServiceClient _service { get; set; }

        private readonly AsyncRetryPolicy<StockModel> _asyncRetryPolicy;

        private ILogger _logger;

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient service, ILogger logger)
        {
            _service = service;
            _asyncRetryPolicy = Policy<StockModel>.Handle<RpcException>()
                    .RetryAsync(3);
            _logger = logger;
        }


        public async Task<StockModel> GetStock(string itemNo)
        {
            Log.Information("Starting GetStock request for item {ItemNo}", itemNo);
            try
            {
                var stockItemRequest = new GetStockRequest { ItemNo = itemNo };
                var result = await _asyncRetryPolicy.ExecuteAsync(async () =>
                {
                    return await _service.GetStockAsync(stockItemRequest);
                });
                Log.Information("Completed GetStock request for item {ItemNo}", itemNo);
                return result;
            }
            catch (RpcException ex)
            {
                Log.Error(ex, "RPC error in GetStock for item {ItemNo}", itemNo);
                return new StockModel { Quantity = -1 };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetStock for item {ItemNo}", itemNo);
                return new StockModel { Quantity = -1 };
            }
        }
    }
}