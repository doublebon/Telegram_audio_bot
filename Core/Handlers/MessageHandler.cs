using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using telegram_audio_bot.Core.Config;
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
                        if (message.Audio != null && message.Chat.Username != null && AppConfig.GetAdminUsernames().Contains(message.Chat.Username.ToLower()))
                        {
                            var infoMsg = await botClient.SendTextMessageAsync(message.Chat, "Got file, start upload...");
                            var file = await botClient.GetFileAsync(message.Audio.FileId);
                            var filesDir = Directory.CreateDirectory($"music/{file.FileId}");

                            var audioFilePath = filesDir.FullName + '/' + file.FileId + "." + file.FilePath.Split('.').Last();

                            try{
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
                            finally{
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
                            await botClient.SendTextMessageAsync(message.Chat, $"Hello! {message.Chat.Username}. I got your message, but for use write @*this_bot_name* in chat!");
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
                    AudioStore.GetActVoiceRecords(),
                    isPersonal: false,
                    cacheTime: 0);
            }
        }

    }
}
