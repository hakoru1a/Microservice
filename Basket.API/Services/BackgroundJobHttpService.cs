using Basket.API.Extensions;
using Shared.Configurations;
using Shared.DTOs.ScheduleJob;

namespace Basket.API.Services
{
    public class BackgroundJobHttpService
    {
        private HttpClient Client;

        private string ScheduledUrl;
        public BackgroundJobHttpService(HttpClient client, BackgroundJobSettings settings) 
        {
            client.BaseAddress = new Uri(settings.HangfireUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json"); 
            Client = client; 
            ScheduledUrl = settings.ScheduleJobUrl;
        }

        public async Task<string> SendEmailReminderCheckout(ReminderCheckoutOrderDto model)
        {
            var uri = $"{ScheduledUrl}/send-email-reminder-checkout-order";
            var response = await Client.PostAsJson(uri, model);
            string jobId = null;

            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                jobId = await response.ReadContentAs<string>();
            }

            return jobId;
        }

        public void DeleteReminderCheckoutOrder(string jobId)
        {
            var uri = $"{ScheduledUrl}/delete/jobId/{jobId}";
            Client.DeleteAsync(uri);
        }
    }
}
