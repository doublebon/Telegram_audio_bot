using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace telegram_audio_bot.Core.Handlers.Commands.Pack
{
    internal class HelloCommand : BaseCommand
    {
        protected override string GetCommandName()
        {
            return @"/hello";
        }

        public override async Task<bool> TryCommandRun(ITelegramBotClient botClient, Message message)
        {
            var isAnswer = await AnswerOnReply(botClient, message);

            if (!isAnswer && IsCommand(message))
            {
                await botClient.SendTextMessageAsync(message.Chat, "Hello!", ParseMode.Markdown, replyMarkup: new ForceReplyMarkup { Selective = false });
                return true;
            }
            return false;
        }
    }
}
