﻿using Shared.Configurations;

namespace Basket.API.Services
{
    public class BackgroundJobHttpService
    {
        public HttpClient Client { get; set; }

        public string ScheduledUrl { get; set; }
        public BackgroundJobHttpService(HttpClient client, BackgroundJobSettings settings) 
        {
            client.BaseAddress = new Uri(settings.HangfireUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json"); 
            Client = client; 
            ScheduledUrl = settings.ScheduleJobUrl;
        }


    }
}
