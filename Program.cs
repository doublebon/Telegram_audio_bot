

using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

class Program
{
    static ITelegramBotClient _bot = new TelegramBotClient("*");

    static void Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
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
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            if (message.Text.ToLower() == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat, "Start hello!");
                return;
            }
            await botClient.SendTextMessageAsync(message.Chat, "Base hello!");
        }
        else if (update.Type == Telegram.Bot.Types.Enums.UpdateType.InlineQuery)
        {
            await botClient.AnswerInlineQueryAsync(
                update.InlineQuery.Id,
                new InlineQueryResultCachedVoice[] { new InlineQueryResultCachedVoice(id: "1", fileId: "AwADAgADBwcAAoKG0Eg5gsPd_smTTBYE", title: "sdasd") },
                isPersonal: false,
                cacheTime: 0);
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }
}