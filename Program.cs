using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using telegram_audio_bot.Core.Handlers;
using telegram_audio_bot.Core.Store;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace telegram_audio_bot;

internal class Program
{
    private static void Main()
    {
        AudioStore.UpdateCachedVoicesList();
        var lastChatMessageId = Bot.GetLastMessageId(); // for offset old messages
        Console.WriteLine(lastChatMessageId);
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            Offset = lastChatMessageId != 0 ? lastChatMessageId + 1 : null, // ignore old messages
            AllowedUpdates = { }, // receive all update types
        };

        Bot.Client.StartReceiving(
            HandleUpdateAsync,
            ErrorHandler.HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );

        Console.ReadLine();
    }

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

        switch (update.Type)
        {
            case UpdateType.Message:
                await MessageHandler.HandleMessage(botClient, update.Message);
                break;
            case UpdateType.InlineQuery:
                await MessageHandler.HandleInline(botClient, update.InlineQuery);
                break;
            default:
                ErrorHandler.HandleError(
                    $"Switch default: unknown message type: \"{update.Type}\"",
                    new ArgumentOutOfRangeException($"At method: {new StackTrace().GetFrame(3)?.GetMethod()?.Name ?? "HandleUpdateAsync"}"));
                break;
        }
    }
}