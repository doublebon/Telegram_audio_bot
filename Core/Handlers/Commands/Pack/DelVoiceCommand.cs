using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using telegram_audio_bot.Core.Handlers.Commands.Attributes;
using telegram_audio_bot.Core.Store;

namespace telegram_audio_bot.Core.Handlers.Commands.Pack
{
    [AdminRequire]
    [BotCommand]
    internal class DelVoiceCommand : BaseCommand
    {
        protected override string GetCommandName()
        {
            return @"/delvoice";
        }

        protected override async Task<bool> AnswerOnReply(ITelegramBotClient botClient, Message message)
        {
            if (message.ReplyToMessage?.MessageId == GetMessageIdForReply())
            {
                await RemoveAudioFromStore(botClient, message);
                AudioStore.UpdateCachedVoicesList();
                return true;
            }
            return false;
        }

        public override async Task<bool> TryCommandRun(ITelegramBotClient botClient, Message message)
        {
            var isAnswer = await AnswerOnReply(botClient, message);

            if (!isAnswer && IsCommand(message))
            {
                SaveMessageForReply(await botClient.SendTextMessageAsync(message.Chat, "Enter title of record for remove", ParseMode.Markdown, replyMarkup: new ForceReplyMarkup { Selective = false }));
            }
            return isAnswer || IsCommand(message);
        }

        private static async Task RemoveAudioFromStore(ITelegramBotClient botClient, Message message)
        {
            AudioStore.RemoveVoiceRecord(message.Text ?? "");
            await botClient.SendTextMessageAsync(message.Chat, $"Voice with title: {message.Text} was removed");
        }

    }
}
