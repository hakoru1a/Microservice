﻿using Constracts.Common.Interface;
using OrderCatalog = Order.Domain.Entities.Order;

namespace Order.Domain.Interfaces
{
    public interface IOrderRepository : IRepositoryBaseAsync<OrderCatalog, long>
    {
        Task<IEnumerable<OrderCatalog>> GetOrdersByUsername(string username);

        Task<OrderCatalog> GetOrderByDocumentNo(string documentNo);
        void CreateOrder(OrderCatalog order);
        void DeleteOrder(OrderCatalog order);
    }
}
