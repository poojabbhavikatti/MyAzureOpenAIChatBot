using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyChatBot.Models;
using MyChatBot.Services;

namespace MyChatBot.Services
{
    public class OpenAIService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public OpenAIService(IConfiguration configuration)
        {
            _apiKey = "ENTER_YOUR_API_KEY";

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://botai.openai.azure.com/")
            };
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
        }

        public async Task<string> GetOpenAIResponse(string userMessage)
        {
            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "user", content = userMessage }
                },
                max_tokens = 150,
                temperature = 0.7,
                frequency_penalty = 0,
                presence_penalty = 0,
                top_p = 0.95,
                stop = (object)null // Ensure stop is explicitly set to null
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

            var response = await _httpClient.PostAsync("ENTER_YOUR_API_URL", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                using (JsonDocument document = JsonDocument.Parse(jsonResponse))
                {
                    // Get the root element
                    JsonElement root = document.RootElement;

                    // Access the 'message' object inside 'choices'
                    JsonElement messageElement = root
                        .GetProperty("choices")[0] // Assuming there's only one choice in the array
                        .GetProperty("message");

                    // Extract the 'content' and 'role' fields
                    string contentValue = messageElement.GetProperty("content").GetString();
                    //string roleValue = "AI Assistant";    //messageElement.GetProperty("role").GetString();

                    return contentValue;
                    // Format the output as required
                    //string output = $"\"content\": \"{contentValue}\", \"role\": \"{roleValue}\"";
                    //string output = contentValue;
                    // Display the formatted output
                    //Console.WriteLine(output);
                    //return output ;

                    //return jsonResponse;


                }
            }
            else
            {
                return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
            }
        }
    }
}
