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

        protected override async void AnswerOnReply(ITelegramBotClient botClient, Message message)
        {
            if (message.ReplyToMessage?.MessageId == GetMessageIdForReply())
            {
                await botClient.SendTextMessageAsync(message.Chat, "Pidor!", ParseMode.Markdown);
            }
        }

        public override async void TryCommandRun(ITelegramBotClient botClient, Message message)
        {
            AnswerOnReply(botClient, message);

            if (this.IsCommand(message))
            {
                SaveMessageForReply(await botClient.SendTextMessageAsync(message.Chat, "Hello!", ParseMode.Markdown, replyMarkup: new ForceReplyMarkup { Selective = false }));
            }
        }
    }
}
