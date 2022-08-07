using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace telegram_audio_bot.Core.Config
{
    public sealed class AppConfig
    {
        private static string BaseConfigName { get; } = "appsettings.json";
        private static JsonNode? JsonConfig { get; } = JsonNode.Parse(File.ReadAllText(BaseConfigName));

        public static string GetTelegramToken()
        {
            return JsonConfig?["TelegramBotToken"]?.AsValue().GetValue<string>() ?? throw new KeyNotFoundException($"Can't find 'TelegramBotToken' key at {BaseConfigName} config file");
        }

        public static List<string> GetAdminUsernames()
        {
            return JsonConfig?["AdminUsernames"]?.AsArray().Select(i => i?.ToString().ToLower() ?? "").ToList() ?? throw new KeyNotFoundException($"Can't find 'AdminUsernames' array key at {BaseConfigName} config file");
        }
    }
}
