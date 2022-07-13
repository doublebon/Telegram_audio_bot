using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InlineQueryResults;

namespace telegram_audio_bot
{
    internal class AudioStore
    {
        public  readonly record struct VoiceTitleAndId(string Title, string FileId);
        private readonly static string AudioStoreFileName = "audioStore.txt";

        private static void CreateAudioStoreFileIfNotExist(){
            using StreamWriter w = File.AppendText(AudioStoreFileName);
            w.Close();
        }

        public static void AppendNewVoiceRecord(string Title, string FileId)
        {
            if(!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(FileId))
            {
                using StreamWriter w = File.AppendText(AudioStoreFileName);
                w.WriteLine($"{Title}:{FileId}");
                w.Close();
            }
        }

        public static void RemoveVoiceRecord(string title)
        {
            CreateAudioStoreFileIfNotExist();

            if (!string.IsNullOrEmpty(title))
            {
                var fileLines = new List<string>(System.IO.File.ReadAllLines(AudioStoreFileName));
                fileLines.RemoveAll(line => line.ToLower().Contains(title.ToLower()));
                File.WriteAllLines(AudioStoreFileName, fileLines.ToArray());
            }
        }

        public static InlineQueryResultCachedVoice[] GetActVoiceRecords()
        {
            CreateAudioStoreFileIfNotExist();

            var audioStoreFileLines = File.ReadAllLines(AudioStoreFileName).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            var audioInfoList = new List<VoiceTitleAndId>(audioStoreFileLines.Length);
            foreach (var line in audioStoreFileLines)
            {
                var splittedLine = line.Split(":");
                audioInfoList.Add(new VoiceTitleAndId(splittedLine[0], splittedLine[1]));
            }

            var voices = new InlineQueryResultCachedVoice[audioStoreFileLines.Length];

            for (int i = 0; i < audioStoreFileLines.Length; i++)
            {
                var parsedFromFile = audioInfoList[i];
                voices[i] = new InlineQueryResultCachedVoice(id: Convert.ToString(i), title: parsedFromFile.Title, fileId: parsedFromFile.FileId);
            }

            return voices;
        }



    }
}
