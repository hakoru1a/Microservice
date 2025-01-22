using Grpc.Core;
using Inventory.gRPC.Protos;
namespace Basket.API.gRPCServices
{
    public class StockItemGrpcService 
    {
        private StockProtoService.StockProtoServiceClient _service {  get; set; }

        public StockItemGrpcService(StockProtoService.StockProtoServiceClient service)
        {
            _service = service;
        }


        public async Task<StockModel> GetStock(string itemNo)
        {
            try
            {
                var stockItemRequest = new GetStockRequest { ItemNo = itemNo };
                return await _service.GetStockAsync(stockItemRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
