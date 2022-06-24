using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using telegram_audio_bot;

class Program
{
    static ITelegramBotClient _bot = new TelegramBotClient("*");

    static void Main(string[] args)
    {
        var lastChatMessageId = getLastMessageId(); // for offset old messages
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            Offset = lastChatMessageId != 0 ? lastChatMessageId + 1 : null, // ignore old messages
            AllowedUpdates = { }, // receive all update types
        };

        _bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }

    private static int getLastMessageId()
    {
        try
        {
            var upds = _bot.GetUpdatesAsync().Result;
            return upds.Length > 0 ? upds[upds.Length - 1].Id : 0;
        }
        catch (Exception e)
        {
            ErrorHandle("Can't get any bot updates. Possibly invalid token.", e, true);
        }
        return 0;
    }

    public static void ErrorHandle(string errorMessage, Exception exeption) => ErrorHandle(errorMessage, exeption, false);
    

    public static void ErrorHandle(string errorMessage, Exception exeption, bool hardQuit)
    {
        Console.WriteLine($"\n*** Error ***\nError: \"{errorMessage}\"\nMessage: {exeption.Message}\nStack Trace:\n {exeption.StackTrace}\n*** Error ***\n");
        if (hardQuit)
        {
            Console.ReadLine();
            Environment.Exit(0);
        }
    }

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

        switch (update.Type)
        {
            case UpdateType.Message:
                await MessageHandler.HandleMessage(_bot, update.Message);
                break;
            case UpdateType.InlineQuery:
                await MessageHandler.HandleInline(_bot, update.InlineQuery);
                break;
            default:
                ErrorHandle(
                    $"Switch default: unknown message type: \"{update.Type}\"", 
                    new ArgumentOutOfRangeException($"At method: { new StackTrace().GetFrame(3)?.GetMethod()?.Name?? "HandleUpdateAsync" }"));
                break;
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exeption, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exeption));
    }
}