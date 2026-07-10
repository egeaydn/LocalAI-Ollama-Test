using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OllamaTest
{
    public class OllamaClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;
        private readonly string _baseUrl;
        private readonly List<object> _conversationHistory;

        public OllamaClient(string model = "llama3", string baseUrl = "http://localhost:11434")
        {
            _httpClient = new HttpClient();
            _model = model;
            _baseUrl = baseUrl;
            _conversationHistory = new List<object>();
        }

        public async Task<string> SendMessageAsync(string userMessage)
        {
            _conversationHistory.Add(new { role = "user", content = userMessage });

            var requestData = new
            {
                model = _model,
                messages = _conversationHistory,
                stream = false
            };

            string jsonPayload = JsonSerializer.Serialize(requestData);
            HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{_baseUrl}/api/chat", httpContent);
            string responseString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseString);
            string aiResponse = doc.RootElement.GetProperty("message").GetProperty("content").GetString();

            _conversationHistory.Add(new { role = "assistant", content = aiResponse });

            return aiResponse;
        }

        public void ClearHistory() => _conversationHistory.Clear();
    }
}
