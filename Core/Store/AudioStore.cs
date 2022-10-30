using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;

namespace telegram_audio_bot.Core.Store
{
    internal class AudioStore
    {
        private static ConcurrentBag<VoiceTitleAndId> CachedVoices = new();
        public readonly record struct VoiceTitleAndId(string Title, string FileId);
        private readonly static string AudioStoreFileName = "audioStore.txt";

        private static void CreateAudioStoreFileIfNotExist()
        {
            File.AppendText(AudioStoreFileName).Close();
        }

        public static void AppendNewVoiceRecord(string Title, string FileId)
        {
            if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(FileId))
            {
                using StreamWriter w = File.AppendText(AudioStoreFileName);
                w.WriteLine($"{Title}:{FileId}");
                w.Close();
            }
        }

        public static void RemoveVoiceRecord(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                var fileLines = new List<string>(File.ReadAllLines(AudioStoreFileName));
                fileLines.RemoveAll(line => line.ToLower().Contains(title.ToLower()));
                File.WriteAllLines(AudioStoreFileName, fileLines.ToArray());
            }
        }

        public static InlineQueryResultCachedVoice[] GetActVoiceRecords()
        {
            return CachedVoices.Select((audio, i) => new InlineQueryResultCachedVoice(id: Convert.ToString(i), title: audio.Title, fileId: audio.FileId)).ToArray();
        }

        public static void UpdateCachedVoicesList()
        {
            CreateAudioStoreFileIfNotExist();
            CachedVoices.Clear();
            File.ReadAllLines(AudioStoreFileName).Where(x => !string.IsNullOrWhiteSpace(x)).Reverse<string>().ToList().ForEach(line =>
            {
                var splittedLine = line.Split(":");
                CachedVoices.Add(new VoiceTitleAndId(splittedLine[0], splittedLine[1]));
            });
        }


    }
}
