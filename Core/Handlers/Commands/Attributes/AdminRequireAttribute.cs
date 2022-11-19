using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace telegram_audio_bot.Core.Handlers.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    class AdminRequireAttribute : Attribute
    {
    }
}
