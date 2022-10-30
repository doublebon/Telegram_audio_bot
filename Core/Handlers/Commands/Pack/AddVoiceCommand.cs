using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using telegram_audio_bot.Core.Store;

namespace telegram_audio_bot.Core.Handlers.Commands.Pack
{
    internal class AddVoiceCommand : BaseCommand
    {
        protected override string GetCommandName()
        {
            return @"/addvoice";
        }

        protected override async Task<bool> AnswerOnReply(ITelegramBotClient botClient, Message message)
        {
            if (message.ReplyToMessage?.MessageId == GetMessageIdForReply())
            {
                await AppendAudioToStore(botClient, message);
                AudioStore.UpdateCachedVoicesList();
                return true;
            }
            return false;
        }

        public override async Task<bool> TryCommandRun(ITelegramBotClient botClient, Message message)
        {
            if (!Bot.IsAdminMessage(message))
            {
                return false;
            }

            var isAnswer = await AnswerOnReply(botClient, message);

            if (!isAnswer && IsCommand(message))
            {
                SaveMessageForReply(await botClient.SendTextMessageAsync(message.Chat, "Add new voice record in format title:fileId", ParseMode.Markdown, replyMarkup: new ForceReplyMarkup { Selective = false }));
            }

            return isAnswer || IsCommand(message);
        }

        private static async Task AppendAudioToStore(ITelegramBotClient botClient, Message message)
        {
            var textLines = message.Text?.Split("\n");
            if (textLines != null && textLines.Length > 0)
            {
                foreach (var line in textLines)
                {
                    var splitMessage = Regex.Replace(line, @"\s{2,}", string.Empty).Split(":");
                    if (splitMessage != null && splitMessage.Length > 1)
                    {
                        AudioStore.AppendNewVoiceRecord(splitMessage.ElementAt(0), splitMessage.ElementAt(1));
                        await botClient.SendTextMessageAsync(message.Chat, $"New voice was added: {splitMessage.ElementAt(0)}:{splitMessage.ElementAt(1)}");
                    }
                }
            }
        }
    }
}