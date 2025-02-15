﻿using Basket.API.Entities;
using Basket.API.Extensions;
using Basket.API.Repository.Interface;
using Basket.API.Services;
using Basket.API.Services.Interfaces;
using Constracts.Common.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Shared.DTOs.ScheduleJob;
using ILogger = Serilog.ILogger;
namespace Basket.API.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private IDistributedCache _redisCache;

        private ISerializeService _serializeService;

        private ILogger _logger; 

        private BackgroundJobHttpService _backgroundJobHttpService;

        private IEmailTemplateService _emailTemplateService;

        public BasketRepository(IDistributedCache cache, ISerializeService serializeService, ILogger logger, 
            BackgroundJobHttpService backgroundJobHttpService, IEmailTemplateService emailTemplateService)
        {
            _redisCache = cache;
            _serializeService = serializeService;
            _logger = logger;
            _backgroundJobHttpService = backgroundJobHttpService;
            _emailTemplateService = emailTemplateService;
        }
        public async Task<bool> DeleteBasket(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            try
            {
                await _redisCache.RemoveAsync(username);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error($"Delete cache for ${username} fail");
                return false;
            }
        }

        public async Task DeleteBasketFromUserName(string username)
        {
            await DeleteReminderCheckoutOrder(username);
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            await _redisCache.RemoveAsync(username);
        }

        public async Task<Cart?> GetBasketByUsername(string username)
        {
            try
            {
                _logger.Information("Test log message");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging failed: {ex.Message}");
            }
            _logger.Information(messageTemplate: $"BEGIN: GetBasketByUserName {username}");
            var basket = await _redisCache.GetStringAsync(username);

            if (!string.IsNullOrEmpty(basket))
            {
                var result = _serializeService.Deserialize<Cart>(basket);
                var totalPrice = result.TotalPrice;
                _logger.Information("END: GetBasketByUserName {username} - Total Price: {totalPrice}", username, totalPrice);
                return result;
            }

            return null;
        }

        public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions options)
        {
            await DeleteReminderCheckoutOrder(cart.Username);

            if (options != null)
            {
                await _redisCache.SetStringAsync(cart.Username,_serializeService.Serialize(cart),options);
            }
            else
            {
                await _redisCache.SetStringAsync(cart.Username, _serializeService.Serialize(cart));
            }

            try
            {
                await TriggerSendEmailReminderCheckout(cart);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }

            var value =  await GetBasketByUsername(cart.Username);

            return value;
        }

        private async Task DeleteReminderCheckoutOrder(string username)
        {
            try
            {
                var cart = await GetBasketByUsername(username);
                if (cart == null || string.IsNullOrEmpty(cart.JobId)) return;

                var jobId = cart.JobId;
               _backgroundJobHttpService.DeleteReminderCheckoutOrder(jobId);
                _logger.Information($"DeleteReminderCheckoutOrder:Deleted JobId: {jobId}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error deleting reminder for user {username}");
                throw;
            }
        }

        private async Task TriggerSendEmailReminderCheckout(Cart cart)
        {
            var emailTemplate = _emailTemplateService.GenerateReminderCheckoutOrderEmail(cart.Username);

            var model = new ReminderCheckoutOrderDto(
                email: cart.Email,
                subject: "Reminder checkout",
                emailContent: emailTemplate,
                enqueueAt: DateTimeOffset.UtcNow.AddSeconds(30)
            );

            var jobId = await _backgroundJobHttpService.SendEmailReminderCheckout(model);

            if (!string.IsNullOrEmpty(jobId))
            {
                cart.JobId = jobId;
                await _redisCache.SetStringAsync(cart.Username, _serializeService.Serialize(cart));
            }
        }
    }
}
