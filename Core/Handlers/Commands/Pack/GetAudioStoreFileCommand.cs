using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using telegram_audio_bot.Core.Store;

namespace telegram_audio_bot.Core.Handlers.Commands.Pack
{
    internal class GetAudioStoreFileCommand : BaseCommand
    {
        protected override string GetCommandName()
        {
            return @"/getstore";
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
                SendAudioStoreFile(botClient, message);
                //await botClient.SendTextMessageAsync(message.Chat, "Voices list was updated", ParseMode.Markdown);
            }

            return isAnswer || IsCommand(message);
        }

        private static async void SendAudioStoreFile(ITelegramBotClient botClient, Message message)
        {

            using var stream = System.IO.File.Open(AudioStore.GetAudioStoreFileName(), FileMode.Open);
            var iof = new InputOnlineFile(stream) { FileName = AudioStore.GetAudioStoreFileName() };
            await botClient.SendDocumentAsync(message.Chat.Id, iof, "");
        }
    }
}
