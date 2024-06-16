using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FanDuel.DepthChart.Test.Extensions
{
    public static class APIHttpExtensions
    {
        public static async Task<TResult> PostAsJsonAsync<TRequest, TResult>(this HttpClient client, string requestUri, TRequest content)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Serialize the request content to JSON
            var jsonContent = JsonSerializer.Serialize(content, jsonOptions);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await client.PostAsync(requestUri, httpContent);
            response.EnsureSuccessStatusCode();

            // Read the response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response content to the specified type
            return JsonSerializer.Deserialize<TResult>(responseContent, jsonOptions);
        }

        public static async Task PostAsJsonAsync<TRequest>(this HttpClient client, string requestUri, TRequest content)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Serialize the request content to JSON
            var jsonContent = JsonSerializer.Serialize(content, jsonOptions);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await client.PostAsync(requestUri, httpContent);
            response.EnsureSuccessStatusCode();
        }

        public static async Task<TResult> GetFromJsonAsync<TResult>(this HttpClient client, string requestUri)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Send the GET request
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            // Read the response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response content to the specified type
            return JsonSerializer.Deserialize<TResult>(responseContent, jsonOptions);
        }
    }
}
