﻿using Saga.Orchestrator.Extensions;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Inventory;

namespace Saga.Orchestrator.HttpRepository
{
    public class IventoryHttpRepository : IInventoryHttpRepository
    {
        public readonly HttpClient _client;

        public IventoryHttpRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> CreateOrderSale(string orderNo, SalesOrderDto model)
        {
            var response = await _client.PostAsJsonAsync(requestUri: $"inventory/sales/order-no/{orderNo}",
                model); //Task<HttpResponseMessage>

            if (!response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                throw new Exception(message: $"Create sale order for Order No: {orderNo} not success");

            var result = await response.ReadContentAs<CreatedSalesOrderSuccessDto>();
            return result.DocumentNo;
        }

        public async Task<string> CreateSalesOrder(SalesProductDto model)
        {
            var response = await _client.PostAsJsonAsync($"inventory/sales/{model.ItemNo}", model);
            if (!response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                throw new Exception($"Create sale order for item: {model.ItemNo} not success");

            var inventory = await response.ReadContentAs<InventoryEntryDto>();
            return inventory.DocumentNo;
        }

        public async Task<bool> DeleteOrderByDocumentNo(string documentNo)
        {
            var response = await _client.DeleteAsync($"inventory/document-no/{documentNo}");
            if (!response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                throw new Exception($"Delete order for Document No: {documentNo} not success");

            var result = await response.ReadContentAs<bool>();
            return result;
        }
    }
}
