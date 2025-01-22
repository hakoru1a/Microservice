using Grpc.Core;
using Inventory.gRPC.Protos;
using Inventory.gRPC.Repositories.Interfaces;
using ILogger = Serilog.ILogger;
namespace Inventory.gRPC.Services
{
    public class InventoryService : StockProtoService.StockProtoServiceBase
    {
        private IIventoryRepository _repository;
        private ILogger _logger;

        public InventoryService(IIventoryRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override async Task<StockModel> GetStock(GetStockRequest request, ServerCallContext context)
        {
            _logger.Information($"BEGIN GET STOCK ITEM No {request.ItemNo}");

            var stockQuantity = await _repository.GetStockEntity(request.ItemNo);
            var result = new StockModel
            {
                Quantity = stockQuantity,
            };

            _logger.Information($"END GET STOCK ITEM No:{request.ItemNo} - Quantity: {stockQuantity}");

            return result;
        }
    }
}
