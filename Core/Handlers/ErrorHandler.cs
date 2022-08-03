using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace telegram_audio_bot.Core.Handlers
{
    public sealed class ErrorHandler
    {
        public static void HandleError(Exception exeption)
        {
            HandleError("", exeption, false);
        }

        public static void HandleError(string errorMessage, Exception exeption)
        {
            HandleError(errorMessage, exeption, false);
        }

        public static void HandleError(string errorMessage, Exception exeption, bool hardQuit)
        {
            Console.WriteLine($"\n*** Error ***\nError: \"{errorMessage}\"\nMessage: {exeption.Message}\nStack Trace:\n {exeption.StackTrace}\n*** Error ***\n");
            if (hardQuit)
            {
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exeption, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync(Newtonsoft.Json.JsonConvert.SerializeObject(exeption));
        }
    }
}
