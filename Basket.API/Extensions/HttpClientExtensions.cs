using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.API.Extensions
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
                throw new ApplicationException(message: $"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content // HttpContent
                .ReadAsStringAsync() // Task<string>
                .ConfigureAwait(false); // ConfiguredTaskAwaitable<string>

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.Preserve
            });
        }
    }
}
