using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using telegram_audio_bot.Core.Handlers.Commands.Pack;

namespace telegram_audio_bot.Core.Handlers.Commands
{
    class CommandController
    {
        private static IReadOnlyList<BaseCommand> CommandsList => new List<BaseCommand>()
        {
            new HelloCommand(),
            //new AddVoiceCommand()
        };

        public static ConcurrentDictionary<long, IReadOnlyList<BaseCommand>> CommandsForUserId = new();

        public static void TryExecCommand(ITelegramBotClient botClient, Message message)
        {
            if(message is not null)
            {
                CommandsForUserId.TryAdd(message.From!.Id, CommandsList);
                foreach (var command in CommandsForUserId[message.From!.Id])
                {
                    command.TryCommandRun(botClient, message);
                }
            }
        }
    }
}
