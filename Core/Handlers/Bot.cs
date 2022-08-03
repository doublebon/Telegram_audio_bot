using System;
using Telegram.Bot;
using telegram_audio_bot.Core.Config;

namespace telegram_audio_bot.Core.Handlers
{
    public static class Bot
    {
        private static readonly TelegramBotClient _client = new(AppConfig.GetTelegramToken());
        public  static TelegramBotClient Client => _client ?? throw new InvalidOperationException($"Can't create TelegramBotClient instance");

        
        public static int GetLastMessageId()
        {
            try
            {
                var upds = Client.GetUpdatesAsync().Result;
                return upds.Length > 0 ? upds[upds.Length - 1].Id : 0;
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError("Can't get any bot updates. Possibly invalid token.", e, true);
            }
            return 0;
        }
    }
}
