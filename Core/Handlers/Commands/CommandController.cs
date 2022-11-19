using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using telegram_audio_bot.Core.Handlers.Commands.Attributes;

namespace telegram_audio_bot.Core.Handlers.Commands
{
    class CommandController
    {
        private static IReadOnlyList<BaseCommand> AdminCommandsList => CreateCommands(type => true);
        private static IReadOnlyList<BaseCommand> UserCommandsList => CreateCommands(type => !type.IsDefined(typeof(AdminRequireAttribute), true));

        //Find all commands classes with [BotCommandAttribute]
        private static readonly Type[] CommandClasses = Assembly.GetExecutingAssembly()
                                                                .GetTypes()
                                                                .Where(t => t.IsSubclassOf(typeof(BaseCommand)) &&
                                                                       t.IsDefined(typeof(BotCommandAttribute), true))
                                                                .ToArray();



        private static IReadOnlyList<BaseCommand> CreateCommands(Func<Type, bool> filterByAttribute)
        {
            return CommandClasses
                .Where(t => t != null)
                .Where(filterByAttribute)
                .Select(target => Activator.CreateInstance(target) as BaseCommand)
                .Cast<BaseCommand>()
                .ToList();
        }

        private static readonly ConcurrentDictionary<long, IReadOnlyList<BaseCommand>> CommandsForUserId = new();

        private static IReadOnlyList<BaseCommand> GetCommandsListByUserPermision(Message message)
        {
            return Bot.IsAdminMessage(message) ? AdminCommandsList : UserCommandsList;
        }

        public static async Task<bool> TryExecCommand(ITelegramBotClient botClient, Message message)
        {
            if (message is not null)
            {
                CommandsForUserId.TryAdd(message.From!.Id, GetCommandsListByUserPermision(message));
                foreach (var command in CommandsForUserId[message.From!.Id])
                {
                    var isCommandWasExecuted = await command.TryCommandRun(botClient, message);
                    if (isCommandWasExecuted)
                        return true;
                }
            }
            return false;
        }
    }
}
