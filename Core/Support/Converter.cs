using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace telegram_audio_bot.Core.Support
{
    public static class Converter
    {
        public async static Task<string> AudioToVoice(string audioPath)
        {
            var voiceFilePath = audioPath.Replace(Path.GetExtension(audioPath), ".ogg");

            var script = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? EnumScripts.FFMPEG_CONVERT_WIN : EnumScripts.FFMPEG_CONVERT_UNIX;
            ProcessStartInfo info = new ProcessStartInfo(script, $"{audioPath} {voiceFilePath}");
            await Process.Start(info).WaitForExitAsync();

            return voiceFilePath;
        }

    }
}
