using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace telegram_audio_bot.Core.Handlers.Commands
{
    public abstract class BaseCommand
    {
        private Message? MessageForReply;

        protected abstract string GetCommandName();
        protected virtual  Task<bool> AnswerOnReply(ITelegramBotClient botClient, Message message){ return false; }
        public    abstract Task<bool> TryCommandRun(ITelegramBotClient botClient, Message message);

        protected bool IsCommand(Message message)
        {
            if (message.Text is not null)
            {
                return message.Text.ToLower().Equals(GetCommandName().ToLower());
            }
            else
            {
                return false;
            }
        }

        protected void SaveMessageForReply(Message message)
        {
            this.MessageForReply = message;
        }

        protected int GetMessageIdForReply()
        {
            return this.MessageForReply?.MessageId ?? -1;
        }
        
    }
}
