﻿using Shared.DTOs.Basket;

namespace Saga.Orchestrator.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<bool> CheckoutOrder(string username, BasketCheckoutDto model);
    }
}
