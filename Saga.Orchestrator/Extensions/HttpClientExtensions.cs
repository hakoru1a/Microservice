﻿using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Saga.Orchestrator.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(url, content);
        }

        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                // For boolean results, return true for success
                if (typeof(T) == typeof(bool))
                    return (T)(object)true;
                return default(T);
            }

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(dataAsString))
            {
                // For boolean results, return true for success
                if (typeof(T) == typeof(bool))
                    return (T)(object)true;
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
    }
    }
}
