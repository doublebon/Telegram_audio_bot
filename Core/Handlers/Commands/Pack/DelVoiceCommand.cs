using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace telegram_audio_bot.Core.Handlers.Commands.Pack
{
    internal class DelVoiceCommand
    {
        public string GetCommandName()
        {
            return @"/delvoice";
        }

        public async void TryCommandRun(ITelegramBotClient botClient, Message message)
        {
            //ICommand command = this;
            //if (command.IsCommand(message))
            //{
            //    await botClient.SendTextMessageAsync(message.Chat, "Hello!", ParseMode.Markdown);
            //}
        }
    }
}
