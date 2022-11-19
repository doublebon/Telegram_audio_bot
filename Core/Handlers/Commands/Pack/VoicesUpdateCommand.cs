using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using telegram_audio_bot.Core.Handlers.Commands.Attributes;
using telegram_audio_bot.Core.Store;

namespace telegram_audio_bot.Core.Handlers.Commands.Pack
{
    [AdminRequire]
    [BotCommand]
    internal class VoicesUpdateCommand : BaseCommand
    {
        protected override string GetCommandName()
        {
            return @"/upd";
        }

        public override async Task<bool> TryCommandRun(ITelegramBotClient botClient, Message message)
        {
            var isAnswer = await AnswerOnReply(botClient, message);

            if (!isAnswer && IsCommand(message))
            {
                AudioStore.UpdateCachedVoicesList();
                await botClient.SendTextMessageAsync(message.Chat, "Voices list was updated", ParseMode.Markdown);
            }

            return isAnswer || IsCommand(message);
        }
    }
}
