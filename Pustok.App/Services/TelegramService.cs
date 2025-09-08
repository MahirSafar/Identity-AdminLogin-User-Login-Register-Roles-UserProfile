using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pustok.App.Services
{
    public class TelegramService
    {
        private readonly string _botToken = "YOUR_BOT_TOKEN";
        private readonly string _chatId = "YOUR_CHAT_ID";

        public async Task SendMessageAsync(string message, string buttonUrl)
        {
            using var client = new HttpClient();

            // Telegram expects this exact JSON format for inline keyboards
            var payload = new
            {
                chat_id = _chatId,
                text = message,
                reply_markup = new
                {
                    inline_keyboard = new[]
                    {
                        new[]
                        {
                            new { text = "View Details", url = buttonUrl }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var apiUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage";
            var response = await client.PostAsync(apiUrl, content);
            response.EnsureSuccessStatusCode();
        }
    }
}
