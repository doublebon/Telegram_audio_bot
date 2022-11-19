using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using telegram_audio_bot.Core.Handlers.Commands;
using telegram_audio_bot.Core.Store;
using telegram_audio_bot.Core.Support;

namespace telegram_audio_bot.Core.Handlers
{
    public static class MessageHandler
    {

        // TODO: will be refactored
        public static async Task HandleMessage(ITelegramBotClient botClient, Message? message)
        {
            if (message != null)
            {

                switch (message.Type)
                {
                    case MessageType.Audio:
                        if (message.Audio != null && message.Chat.Username != null && Bot.IsAdminMessage(message))
                        {
                            var infoMsg = await botClient.SendTextMessageAsync(message.Chat, "Got file, start upload...");
                            var file = await botClient.GetFileAsync(message.Audio.FileId);
                            var filesDir = Directory.CreateDirectory($"music/{file.FileId}");

                            var audioFilePath = filesDir.FullName + '/' + file.FileId + "." + file.FilePath.Split('.').Last();

                            try
                            {
                                using (var createStream = new FileStream(audioFilePath, FileMode.Create))
                                {
                                    await botClient.DownloadFileAsync(file.FilePath, createStream);
                                }

                                infoMsg = await botClient.EditMessageTextAsync(message.Chat, infoMsg.MessageId, "File uploaded. Start convert to voice...");
                                var convertFile = Converter.AudioToVoice(audioFilePath);

                                using (var openStream = new FileStream(await convertFile, FileMode.Open))
                                {
                                    await botClient.EditMessageTextAsync(message.Chat, infoMsg.MessageId, "Voice file | FileId: ");
                                    var uploadedVoice = await botClient.SendVoiceAsync(message.Chat, new InputOnlineFile(openStream));
                                    await botClient.SendTextMessageAsync(message.Chat, uploadedVoice?.Voice?.FileId ?? "");
                                }
                            }
                            finally
                            {
                                filesDir.Delete(true);
                            }
                        }
                        break;
                    case MessageType.Voice:
                        await botClient.SendTextMessageAsync(message.Chat, message.Voice?.FileId ?? "");
                        break;
                    case MessageType.Text:
                        var isCommand = await CommandController.TryExecCommand(botClient, message);
                        if (!isCommand)
                        {
                            InlineKeyboardButton button = InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Try me!");
                            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(button);
                            await botClient.SendTextMessageAsync(message.Chat, $"Hello, {message.Chat.Username}! \nFor use this bot write <code>@{(await botClient.GetMeAsync()).Username}</code> + hit *space* as message to any chat!", replyMarkup: keyboard, parseMode: ParseMode.Html);
                        }
                        break;
                }
            }
        }
        public static async Task HandleInline(ITelegramBotClient botClient, InlineQuery? inlineMessage)
        {
            if (inlineMessage is not null)
            {
                await botClient.AnswerInlineQueryAsync(
                    inlineMessage.Id,
                    AudioStore.GetActVoiceRecords(inlineMessage.Query),
                    isPersonal: false,
                    cacheTime: 0);
            }
        }

    }
}
